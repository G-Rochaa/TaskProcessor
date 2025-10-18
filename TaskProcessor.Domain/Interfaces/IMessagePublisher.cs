namespace TaskProcessor.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        #region Public Methods

        Task PublicarAsync<T>(T mensagem, string nomeFila) where T : class;

        #endregion Public Methods
    }
}
