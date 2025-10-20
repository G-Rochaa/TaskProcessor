using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaRetryService
    {
        Task<IEnumerable<Tarefa>> ObterTarefasParaRetryAsync();
        Task<bool> TarefaDeveSerReprocessadaAsync(Tarefa tarefa);
        Task ProcessarTarefasPendentesAsync();
    }
}
