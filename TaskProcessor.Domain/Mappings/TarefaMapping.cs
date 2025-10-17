using TaskProcessor.Domain.DTOs;
using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Mappings
{
    public static class TarefaMapping
    {
        public static TarefaResponse ToResponse(Tarefa tarefa)
        {
            return new TarefaResponse
            {
                Id = tarefa.Id,
                TipoTarefa = tarefa.TipoTarefa,
                DadosTarefa = tarefa.DadosTarefa,
                Status = tarefa.Status,
                NumeroTentativas = tarefa.NumeroTentativas,
                MaximoTentativas = tarefa.MaximoTentativas,
                DtCriacao = tarefa.DtCriacao,
                DtAtualizacao = tarefa.DtAtualizacao,
                DtProcessamento = tarefa.DtProcessamento,
                MensagemErro = tarefa.MensagemErro
            };
        }

        public static ProcessarTarefaMessage ToMessage(Tarefa tarefa)
        {
            return new ProcessarTarefaMessage
            {
                TarefaId = tarefa.Id,
                TipoTarefa = tarefa.TipoTarefa,
                DadosTarefa = tarefa.DadosTarefa,
                NumeroTentativas = tarefa.NumeroTentativas,
                MaximoTentativas = tarefa.MaximoTentativas
            };
        }

        public static ProcessarTarefaMessage ToMessage(TarefaResponse tarefaResponse)
        {
            return new ProcessarTarefaMessage
            {
                TarefaId = tarefaResponse.Id,
                TipoTarefa = tarefaResponse.TipoTarefa,
                DadosTarefa = tarefaResponse.DadosTarefa,
                NumeroTentativas = tarefaResponse.NumeroTentativas,
                MaximoTentativas = tarefaResponse.MaximoTentativas
            };
        }
    }
}
