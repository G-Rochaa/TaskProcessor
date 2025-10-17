using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITaskProcessor
    {
        bool PodeProcessar(string tipoTarefa);
        Task ProcessarAsync(ProcessarTarefaMessage mensagem);
    }
}
