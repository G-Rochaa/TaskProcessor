using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.Application.Interfaces
{
    public interface ITarefaAppService
    {
        Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest request);
        Task<TarefaResponse?> ObterTarefaAsync(Guid id);
        Task<IEnumerable<TarefaResponse>> ListarTarefasAsync();
        Task<IEnumerable<TarefaResponse>> ListarTarefasPendentesAsync();
    }
}
