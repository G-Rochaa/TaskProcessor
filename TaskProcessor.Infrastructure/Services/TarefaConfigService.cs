using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Infrastructure.Settings;

namespace TaskProcessor.Infrastructure.Services
{
    public class TarefaConfigService : ITarefaConfigService
    {
        private readonly TarefaSettings _settings;
        private readonly ILogger<TarefaConfigService> _logger;

        public TarefaConfigService(IOptions<TarefaSettings> settings, ILogger<TarefaConfigService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public int ObterMaximoTentativas(string tipoTarefa)
        {
            if (_settings.MaximoTentativasPorTipo.TryGetValue(tipoTarefa, out var maxTentativas))
            {
                _logger.LogDebug("Usando configuração específica para tipo '{TipoTarefa}': {MaxTentativas} tentativas", 
                    tipoTarefa, maxTentativas);
                return ValidarMaximoTentativas(maxTentativas);
            }

            _logger.LogDebug("Usando configuração padrão para tipo '{TipoTarefa}': {MaxTentativas} tentativas", 
                tipoTarefa, _settings.MaximoTentativasPadrao);
            
            return ValidarMaximoTentativas(_settings.MaximoTentativasPadrao);
        }

        private int ValidarMaximoTentativas(int maxTentativas)
        {
            var valorValidado = Math.Max(1, Math.Min(maxTentativas, 10));
            
            if (valorValidado != maxTentativas)
            {
                _logger.LogWarning("Valor de máximo tentativas ajustado de {Original} para {Validado}", 
                    maxTentativas, valorValidado);
            }
            
            return valorValidado;
        }
    }
}
