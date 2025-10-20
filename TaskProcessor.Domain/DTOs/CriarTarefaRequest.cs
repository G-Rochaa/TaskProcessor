namespace TaskProcessor.Domain.DTOs
{
    public class CriarTarefaRequest
    {
        public string TipoTarefa { get; set; } = string.Empty;
        public string DadosTarefa { get; set; } = string.Empty;
    }
}
