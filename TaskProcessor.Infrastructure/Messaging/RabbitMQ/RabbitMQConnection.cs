using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly RabbitMQSettings _settings;
        private IConnection? _connection;
        private IModel? _channel;
        private bool _disposed = false;

        public RabbitMQConnection(IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
        }

        public IModel GetChannel()
        {
            if (_channel == null || _channel.IsClosed)
            {
                _channel = GetConnection().CreateModel();
            }
            return _channel;
        }

        private IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                _connection = factory.CreateConnection();
            }
            return _connection;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _channel?.Close();
                _channel?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
