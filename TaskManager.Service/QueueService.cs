using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
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
        private readonly ConnectionFactory _connectionFactory;

        public QueueService(
            ILogger<QueueService> logger, 
            IKanbanTaskRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_NAME") ?? "KanbanTaskQueue";
            _connectionFactory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING") };
        }

        public Task StartProcess(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                var channel = this.CreateChannel();
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

        private IModel CreateChannel()
        {
            var connection = _connectionFactory.CreateConnection();
            return connection.CreateModel();
        }

        public void InsertTask(KanbanTask kanbanTask)
        {
            using var channel = CreateChannel();
            var kanbanTaskEvent = new KanbanTaskEvent { KanbanTask = kanbanTask, EventType = EventType.Insert };
            var json = JsonSerializer.Serialize(kanbanTaskEvent);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: _queueName,
                                basicProperties: null,
                                body: body);
        }

        public void UpdateTask(KanbanTask kanbanTask)
        {
            using var channel = CreateChannel();
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
            using var channel = CreateChannel();
            var kanbanTaskEvent = new KanbanTaskEvent { KanbanTask = new KanbanTask { TaskId = kanbanTaskId }, EventType = EventType.Delete };
            var json = JsonSerializer.Serialize(kanbanTaskEvent);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: _queueName,
                                basicProperties: null,
                                body: body);
        }
    }
}
