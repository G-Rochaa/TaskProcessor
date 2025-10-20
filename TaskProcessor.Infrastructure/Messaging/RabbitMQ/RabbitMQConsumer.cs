using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConsumer : IMessageConsumer, IDisposable
    {
        private readonly RabbitMQConnection _connection;
        private readonly RabbitMQSettings _settings;
        private IModel? _channel;
        private string? _consumerTag;
        private bool _disposed = false;

        public RabbitMQConsumer(RabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
        {
            _connection = connection;
            _settings = settings.Value;
        }

        public async Task IniciarConsumoAsync<T>(string nomeFila, Func<T, Task> manipuladorMensagem) where T : class
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMQConsumer));

            try
            {
                _channel = _connection.GetChannel();

                _channel.QueueDeclare(
                    queue: nomeFila,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                _channel.BasicQos(
                    prefetchSize: 0,
                    prefetchCount: 1,
                    global: false
                );

                var consumer = new EventingBasicConsumer(_channel);  //aqui vou criar o consumer

                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var mensagem = JsonSerializer.Deserialize<T>(json);

                        if (mensagem != null)
                        {
                            await manipuladorMensagem(mensagem);
                        }

                        if (_channel != null && !_channel.IsClosed)
                        {
                            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                        
                        if (_channel != null && !_channel.IsClosed)
                        {
                            try
                            {
                                _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                            }
                            catch (Exception nackEx)
                            {
                                Console.WriteLine($"Erro ao fazer NACK: {nackEx.Message}");
                            }
                        }
                    }
                };

                _consumerTag = _channel.BasicConsume(
                    queue: nomeFila,
                    autoAck: false,
                    consumer: consumer
                );

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar consumo: {ex.Message}");
                throw;
            }
        }

        public async Task PararConsumoAsync()
        {
            if (_disposed)
                return;

            try
            {
                if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    if (!_channel.IsClosed)
                    {
                        _channel.BasicCancel(_consumerTag);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao parar consumo: {ex.Message}");
            }

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    if (_channel != null && !_channel.IsClosed)
                    {
                        _channel.Close();
                        _channel.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao dispose do channel: {ex.Message}");
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}
