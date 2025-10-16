using TaskProcessor.Domain.Enums;

namespace TaskProcessor.Domain.Entities
{
    public class Tarefa
    {
        #region Public Properties

        public Guid Id { get; private set; }
        public string TipoTarefa { get; private set; } = string.Empty;
        public string DadosTarefa { get; private set; } = string.Empty;
        public StatusTarefaEnum Status { get; private set; }
        public int NumeroTentativas { get; private set; }
        public int MaximoTentativas { get; private set; }
        public DateTime DtCriacao { get; private set; }
        public DateTime DtAtualizacao { get; private set; }
        public DateTime? DtProcessamento { get; private set; }
        public string? MensagemErro { get; private set; }

        #endregion Public Properties

        #region Public Constructor

        public Tarefa(string tipoTarefa, string dadosTarefa, int maximoTentativas = 3)
        {
            if (string.IsNullOrWhiteSpace(tipoTarefa))
                throw new ArgumentException("O tipo da tarefa não pode ser vazio.", nameof(tipoTarefa));

            if (string.IsNullOrWhiteSpace(dadosTarefa))
                throw new ArgumentException("Os dados da tarefa não podem ser vazios.", nameof(dadosTarefa));

            Id = Guid.NewGuid();
            TipoTarefa = tipoTarefa;
            DadosTarefa = dadosTarefa;
            Status = StatusTarefaEnum.Pendente;
            NumeroTentativas = 0;
            MaximoTentativas = maximoTentativas;
            DtCriacao = DateTime.UtcNow;
            DtAtualizacao = DateTime.UtcNow;
        }

        #endregion Public Constructor

        #region Public Methods


        public void MarcarComoEmProcessamento()
        {
            if (Status != StatusTarefaEnum.Pendente)
                throw new InvalidOperationException($"Tarefa não pode ser marcada como em processamento. Status atual: {Status}");

            Status = StatusTarefaEnum.EmProcessamento;
            DtAtualizacao = DateTime.UtcNow;
            DtProcessamento = DateTime.UtcNow;
        }

        public void MarcarComoConcluida()
        {
            if (Status != StatusTarefaEnum.EmProcessamento)
                throw new InvalidOperationException($"Tarefa não pode ser marcada como concluída. Status atual: {Status}");

            Status = StatusTarefaEnum.Concluida;
            DtAtualizacao = DateTime.UtcNow;
        }

        public void MarcarComoFalhou(string mensagemErro)
        {
            if (Status != StatusTarefaEnum.EmProcessamento)
                throw new InvalidOperationException($"Tarefa não pode ser marcada como falhou. Status atual: {Status}");

            NumeroTentativas++;
            MensagemErro = mensagemErro;
            DtAtualizacao = DateTime.UtcNow;

            if (NumeroTentativas >= MaximoTentativas)
            {
                Status = StatusTarefaEnum.Falhou;
            }
            else
            {
                Status = StatusTarefaEnum.Pendente;
                DtProcessamento = null;
            }
        }


        public bool PodeSerProcessada()
        {
            return Status == StatusTarefaEnum.Pendente && NumeroTentativas < MaximoTentativas;
        }


        public bool FalhouDefinitivamente()
        {
            return Status == StatusTarefaEnum.Falhou;
        }

        #endregion Public Methods
    }

}
