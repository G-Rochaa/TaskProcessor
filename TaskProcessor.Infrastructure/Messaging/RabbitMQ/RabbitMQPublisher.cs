using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQPublisher : IMessagePublisher
    {
        private readonly RabbitMQConnection _connection;
        private readonly RabbitMQSettings _settings;

        public RabbitMQPublisher(RabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
        {
            _connection = connection;
            _settings = settings.Value;
        }

        public async Task PublicarAsync<T>(T mensagem, string nomeFila) where T : class
        {
            using var channel = _connection.GetChannel(); //peguei o canal
            
            channel.QueueDeclare(    //vou criar a fila
                queue: nomeFila,
                durable: true,       
                exclusive: false,     
                autoDelete: false,    
                arguments: null
            );

            var json = JsonSerializer.Serialize(mensagem); //vou serializar a mensagem
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; 

            channel.BasicPublish(   //vou enviar ar a mensagem
                exchange: "",        
                routingKey: nomeFila,
                basicProperties: properties,
                body: body
            );

            await Task.CompletedTask;
        }

    }
}
