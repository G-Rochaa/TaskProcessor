namespace TaskProcessor.Application.Interfaces
{
    public interface ITarefaRetryAppService
    {
        Task ProcessarTarefasPendentesAsync();
        Task<int> ObterQuantidadeTarefasPendentesAsync();
    }
}
