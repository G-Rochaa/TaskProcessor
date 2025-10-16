using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaRepository : IRepository<Tarefa>
    {
        Task<IEnumerable<Tarefa>> GetTarefasPendentesAsync();
        Task<IEnumerable<Tarefa>> GetTarefasPorStatusAsync(Enums.StatusTarefaEnum status);
        Task<IEnumerable<Tarefa>> GetTarefasPorTipoAsync(string tipoTarefa);
    }
}
