using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConnection : IDisposable
    {
        private readonly RabbitMQSettings _settings;
        private readonly object _lock = new object();
        private IConnection? _connection;
        private bool _disposed = false;

        public RabbitMQConnection(IOptions<RabbitMQSettings> settings)
        {
            _settings = settings.Value;
        }

        public IModel GetChannel()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMQConnection));

            lock (_lock)
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
                        NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                        RequestedHeartbeat = TimeSpan.FromSeconds(60),
                        RequestedConnectionTimeout = TimeSpan.FromSeconds(30)
                    };

                    _connection = factory.CreateConnection();
                }

                return _connection.CreateModel();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_lock)
                {
                    try
                    {
                        _connection?.Close();
                        _connection?.Dispose();
                    }
                    catch
                    {
                        // ignorar erros por enquanto que não sei o que fazer
                    }
                    finally
                    {
                        _disposed = true;
                    }
                }
            }
        }
    }
}
