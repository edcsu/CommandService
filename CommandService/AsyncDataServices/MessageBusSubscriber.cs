using CommandService.Business.Config;
using CommandService.Business.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, args) =>
            {
                _logger.LogInformation("Event received");

                var body = args.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            
            return Task.CompletedTask;
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
