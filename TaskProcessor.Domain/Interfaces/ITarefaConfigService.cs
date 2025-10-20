namespace TaskProcessor.Domain.Interfaces
{
    public interface ITarefaConfigService
    {
        int ObterMaximoTentativas(string tipoTarefa);
    }
}
