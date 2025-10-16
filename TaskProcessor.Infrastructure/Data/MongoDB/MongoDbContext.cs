using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Data.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Tarefa> Tarefas => _database.GetCollection<Tarefa>("Tarefas");
    }
}
