using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TaskManager.Domain.Enum;
using TaskManager.Domain.Model;
using TaskManager.Domain.Repository;
using TaskManager.Domain.Services;

namespace TaskManager.Service
{
    public class QueueService : IQueueService
    {
        private readonly ILogger<QueueService> _logger;
        private readonly IKanbanTaskRepository _repository;
        private readonly string _queueName;
        private readonly int _maxRetries;
        private readonly ConnectionFactory _connectionFactory;

        public QueueService(
            ILogger<QueueService> logger, 
            IKanbanTaskRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_NAME") ?? "KanbanTaskQueue";
            _maxRetries = Int16.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_RETRIES"), out short maxRt) ? maxRt : 2;
            _connectionFactory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING") ?? "localhost",
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_USER") ?? "guest",
                Password = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_PASS") ?? "guest",
                VirtualHost = "/",
            };
        }

        public Task StartProcess(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                var channel = this.EstablishChannel();
                channel.QueueDeclare(queue: _queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, deliverEventArgs) =>
                {
                    try
                    {
                        var message = Encoding.UTF8.GetString(deliverEventArgs.Body.Span);
                        var newEvent = JsonSerializer.Deserialize<KanbanTaskEvent>(message);

                        if (newEvent != null)
                        {

                            switch (newEvent.EventType)
                            {
                                case EventType.Insert:
                                    _repository.Insert(newEvent.KanbanTask);
                                    break;

                                case EventType.Update:
                                    _repository.Update(newEvent.KanbanTask);
                                    break;

                                case EventType.Delete:
                                    _repository.Delete(newEvent.KanbanTask.TaskId);
                                    break;
                            }
                        }

                        channel.BasicAck(deliverEventArgs.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(deliverEventArgs.DeliveryTag, false, true);
                        _logger.LogError($"Erro ao processar evento. RoutingKey: {deliverEventArgs.RoutingKey}. ConsumerTag: {deliverEventArgs.ConsumerTag}. DeliveryTag: {deliverEventArgs.DeliveryTag}", ex);
                    }
                };

                channel.BasicConsume(queue: _queueName,
                                        autoAck: false,
                                        consumer: consumer);
            }

            return Task.CompletedTask;
        }

        public void InsertTask(KanbanTask kanbanTask)
        {
            using var channel = EstablishChannel();
            var kanbanTaskEvent = new KanbanTaskEvent { KanbanTask = kanbanTask, EventType = EventType.Insert };
            var json = JsonSerializer.Serialize(kanbanTaskEvent);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: _queueName,
                                basicProperties: null,
                                body: body);
        }

        public void UpdateTask(int kanbanTaskId, KanbanTask kanbanTask)
        {
            kanbanTask.TaskId = kanbanTaskId;

            using var channel = EstablishChannel();
            var kanbanTaskEvent = new KanbanTaskEvent { KanbanTask = kanbanTask, EventType = EventType.Update };
            var json = JsonSerializer.Serialize(kanbanTaskEvent);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                    routingKey: _queueName,
                                    basicProperties: null,
                                    body: body);
        }

        public void DeleteTask(int kanbanTaskId)
        {
            using var channel = EstablishChannel();
            var kanbanTaskEvent = new KanbanTaskEvent { KanbanTask = new KanbanTask { TaskId = kanbanTaskId }, EventType = EventType.Delete };
            var json = JsonSerializer.Serialize(kanbanTaskEvent);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: _queueName,
                                basicProperties: null,
                                body: body);
        }

        private IModel EstablishChannel()
        {
            var _retrySecondsInterval = 5;
            var retryPolicy = Policy.Handle<BrokerUnreachableException>()
                .WaitAndRetry(retryCount: _maxRetries, sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(attemptCount * _retrySecondsInterval),
                onRetry: (exception, sleepDuration, attemptNumber, context) =>
                {
                    _logger.LogInformation($"Exception error. Retrying in {sleepDuration}. {attemptNumber} / {_maxRetries}. Exception-Message: {exception.Message}");
                });

            var execution = retryPolicy.ExecuteAndCapture(() =>
            {
                return _connectionFactory
                .CreateConnection()
                .CreateModel();
            });

            return execution.Result;
        }
    }
}
