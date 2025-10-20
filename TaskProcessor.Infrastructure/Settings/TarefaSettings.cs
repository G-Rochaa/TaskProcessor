namespace TaskProcessor.Infrastructure.Settings
{
    public class TarefaSettings
    {
        public int MaximoTentativasPadrao { get; set; } = 3;
        public Dictionary<string, int> MaximoTentativasPorTipo { get; set; } = new();
    }
}
