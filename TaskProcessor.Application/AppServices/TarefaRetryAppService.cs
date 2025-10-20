using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Application.AppServices
{
    public class TarefaRetryAppService : ITarefaRetryAppService
    {
        #region Private Fields

        private readonly ITarefaRetryService _tarefaRetryService;

        #endregion Private Fields

        #region Public Constructor

        public TarefaRetryAppService(ITarefaRetryService tarefaRetryService)
        {
            _tarefaRetryService = tarefaRetryService;
        }

        #endregion Public Constructor

        #region Public Methods

        public Task ProcessarTarefasPendentesAsync()
        {
            return _tarefaRetryService.ProcessarTarefasPendentesAsync();
        }

        public async Task<int> ObterQuantidadeTarefasPendentesAsync()
        {
            var tarefasParaRetry = await _tarefaRetryService.ObterTarefasParaRetryAsync();
            return tarefasParaRetry.Count();
        }

        #endregion Public Methods
    }
}