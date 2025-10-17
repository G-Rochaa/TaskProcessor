using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Mappings;

namespace TaskProcessor.Domain.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<TarefaResponse> CriarTarefaAsync(CriarTarefaRequest request)
        {
            var tarefa = Tarefa.Criar(request);
            await _tarefaRepository.AddAsync(tarefa);
            return TarefaMapping.ToResponse(tarefa);
        }

        public async Task<TarefaResponse?> ObterTarefaPorIdAsync(Guid id)
        {
            var tarefa = await _tarefaRepository.GetByIdAsync(id);
            
            if (tarefa == null)
                return null;

            return TarefaMapping.ToResponse(tarefa);
        }

        public async Task<IEnumerable<TarefaResponse>> ListarTarefasAsync()
        {
            var tarefas = await _tarefaRepository.GetAllAsync();
            return tarefas.Select(TarefaMapping.ToResponse);
        }

        public async Task<IEnumerable<TarefaResponse>> ListarTarefasPendentesAsync()
        {
            var tarefas = await _tarefaRepository.GetTarefasPendentesAsync();
            return tarefas.Select(TarefaMapping.ToResponse);
        }

    }
}
