namespace TaskProcessor.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublicarAsync<T>(T mensagem, string nomeFila) where T : class;
        Task PublicarAsync<T>(T mensagem, string exchange, string chaveRoteamento) where T : class;
    }
}
