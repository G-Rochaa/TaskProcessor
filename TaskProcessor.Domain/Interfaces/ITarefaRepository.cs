using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaRepository : IBaseRepository<Tarefa>
    {
        #region Public Methods

        Task<IEnumerable<Tarefa>> GetTarefasPendentesAsync();
        Task<IEnumerable<Tarefa>> GetTarefasPorStatusAsync(Enums.StatusTarefaEnum status);
        Task<IEnumerable<Tarefa>> GetTarefasPorTipoAsync(string tipoTarefa);

        #endregion Public Methods
    }
}
