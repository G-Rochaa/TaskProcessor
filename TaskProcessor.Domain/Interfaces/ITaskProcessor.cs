using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.Domain.Interfaces
{
    public interface ITaskProcessor
    {
        #region Public Methods

        bool PodeProcessar(string tipoTarefa);
        Task ProcessarAsync(ProcessarTarefaMessage mensagem);

        #endregion Public Methods
    }
}
