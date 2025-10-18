using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaService
    {
        #region Public Methods

        Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest request);
        Task<TarefaResponse?> ObterTarefaPorIdAsync(Guid id);
        Task<IEnumerable<TarefaResponse>> ListarTarefasAsync();
        Task<IEnumerable<TarefaResponse>> ListarTarefasPendentesAsync();

        #endregion Public Methods
    }
}
