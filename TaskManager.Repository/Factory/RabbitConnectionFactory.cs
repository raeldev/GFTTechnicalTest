using RabbitMQ.Client;

namespace TaskManager.Repository.Factory
{
    public static class RabbitConnectionFactory
    {
        public static IConnection GetRabbitMqConnection(IServiceProvider _)
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING"),
                Port = AmqpTcpEndpoint.UseDefaultPort
            };
            return factory.CreateConnection();
        }
    }
}
