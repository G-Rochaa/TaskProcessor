using MongoDB.Driver;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Enums;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Infrastructure.Data.MongoDB.Repositories
{
    public class TarefaRepository : BaseRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(IMongoCollection<Tarefa> collection) : base(collection)
        {
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasPendentesAsync()
        {
            var filter = Builders<Tarefa>.Filter.Eq(t => t.Status, StatusTarefaEnum.Pendente);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasPorStatusAsync(StatusTarefaEnum status)
        {
            var filter = Builders<Tarefa>.Filter.Eq(t => t.Status, status);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasPorTipoAsync(string tipoTarefa)
        {
            var filter = Builders<Tarefa>.Filter.Eq(t => t.TipoTarefa, tipoTarefa);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
