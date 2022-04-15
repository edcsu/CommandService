using CommandService.Business.Config;
using CommandService.Business.EventProcessor;
using RabbitMQ.Client;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private const string exchangeName = "trigger";

        public MessageBusSubscriber(IConfiguration configuration, 
            IEventProcessor eventProcessor, 
            ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _logger = logger;

            InitializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        private void InitializeRabbitMQ()
        {
            var rabbitmqConfig = _configuration.GetRabbitMQConfig();
            var factory = new ConnectionFactory()
            {
                HostName = rabbitmqConfig.Host,
                Port = rabbitmqConfig.Port,
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: exchangeName,
                routingKey: string.Empty);

            _logger.LogInformation("Listening on the message bus");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
            _logger.LogInformation("Channel disposed");
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Connection shutdown");
        }

    }
}
