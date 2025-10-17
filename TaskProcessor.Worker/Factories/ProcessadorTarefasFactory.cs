using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Worker.Processors;

namespace TaskProcessor.Worker.Factories
{
    public class ProcessadorTarefasFactory
    {
        private readonly IEnumerable<ITaskProcessor> _processors;
        private readonly ILogger<ProcessadorTarefasFactory> _logger;

        public ProcessadorTarefasFactory(IEnumerable<ITaskProcessor> processors, ILogger<ProcessadorTarefasFactory> logger)
        {
            _processors = processors;
            _logger = logger;
        }

        public ITaskProcessor CriarProcessor(string tipoTarefa)
        {
            _logger.LogDebug("Buscando processador para tipo de tarefa: {TipoTarefa}", tipoTarefa);

            var processor = _processors.FirstOrDefault(p => p.PodeProcessar(tipoTarefa));

            if (processor == null)
            {
                _logger.LogWarning("Nenhum processador específico encontrado para '{TipoTarefa}'. Usando processador padrão.", tipoTarefa);
                processor = _processors.OfType<PadraoTarefaProcessador>().FirstOrDefault();
            }

            if (processor == null)
            {
                throw new InvalidOperationException($"Nenhum processador disponível para tipo '{tipoTarefa}'");
            }

            _logger.LogDebug("Processador selecionado: {ProcessorType} para tipo '{TipoTarefa}'", 
                processor.GetType().Name, tipoTarefa);

            return processor;
        }
    }
}
