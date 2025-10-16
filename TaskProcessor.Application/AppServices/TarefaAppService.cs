using TaskProcessor.Application.Mappings;
using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Application.AppServices
{
    public class TarefaAppService
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMessagePublisher _messagePublisher;

        public TarefaAppService(ITarefaService tarefaService, IMessagePublisher messagePublisher)
        {
            _tarefaService = tarefaService;
            _messagePublisher = messagePublisher;
        }

        public async Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest request)
        {
            var tarefaResponse = await _tarefaService.CriarTarefaAsync(request);

            var mensagem = TarefaMapping.ToMessage(tarefaResponse);

            await _messagePublisher.PublicarAsync(mensagem, "tarefas-para-processar");

            return tarefaResponse;
        }

        public async Task<TarefaResponse?> ObterTarefaAsync(Guid id)
        {
            return await _tarefaService.ObterTarefaPorIdAsync(id);
        }

        public async Task<IEnumerable<TarefaResponse>> ListarTarefasAsync()
        {
            return await _tarefaService.ListarTarefasAsync();
        }

        public async Task<IEnumerable<TarefaResponse>> ListarTarefasPendentesAsync()
        {
            return await _tarefaService.ListarTarefasPendentesAsync();
        }
    }
}
