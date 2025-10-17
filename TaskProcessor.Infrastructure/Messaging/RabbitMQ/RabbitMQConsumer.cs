using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConsumer : IMessageConsumer
    {
        private readonly RabbitMQConnection _connection;
        private readonly RabbitMQSettings _settings;
        private IModel? _channel;
        private string? _consumerTag;

        public RabbitMQConsumer(RabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
        {
            _connection = connection;
            _settings = settings.Value;
        }

        public async Task IniciarConsumoAsync<T>(string nomeFila, Func<T, Task> manipuladorMensagem) where T : class
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

            var consumer = new EventingBasicConsumer(_channel); //vou criar o consumer

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

                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _consumerTag = _channel.BasicConsume(
                queue: nomeFila,
                autoAck: false,
                consumer: consumer
            );

            await Task.CompletedTask;
        }

        public async Task PararConsumoAsync()
        {
            if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
            {
                _channel.BasicCancel(_consumerTag);
                _channel.Close();
                _channel.Dispose();
            }

            await Task.CompletedTask;
        }
    }
}
