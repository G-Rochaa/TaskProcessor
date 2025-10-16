using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaService
    {
        Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest request);
        Task<TarefaResponse?> ObterTarefaPorIdAsync(Guid id);
        Task<IEnumerable<TarefaResponse>> ListarTarefasAsync();
        Task<IEnumerable<TarefaResponse>> ListarTarefasPendentesAsync();
    }
}
