using Microsoft.AspNetCore.Mvc;
using TaskProcessor.Application.AppServices;
using TaskProcessor.Domain.DTOs;

namespace TaskProcessor.API.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    public class TarefasController : ControllerBase
    {
        #region Private Fields
        private readonly TarefaAppService _tarefaAppService;

        #endregion Private Fields

        #region Public Constructor
        public TarefasController(TarefaAppService tarefaAppService)
        {
            _tarefaAppService = tarefaAppService;
        }

        #endregion Public Constructor

        #region Public Methods

        [HttpPost]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TarefaResponse>> CriarTarefaAsync([FromBody] CriarTarefaRequest request)
        {
            try
            {
                var tarefa = await _tarefaAppService.CriarTarefaAsync(request);
                return CreatedAtAction(nameof(ObterTarefaAsync), new { id = tarefa.Id }, tarefa);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TarefaResponse>> ObterTarefaAsync(Guid id)
        {
            var tarefa = await _tarefaAppService.ObterTarefaAsync(id);
            
            if (tarefa == null)
                return NotFound(new { message = "Tarefa não encontrada" });

            return Ok(tarefa);
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TarefaResponse>>> ListarTarefaAsync()
        {
            var tarefas = await _tarefaAppService.ListarTarefasAsync();
            return Ok(tarefas);
        }

        [HttpGet("pendentes")]
        [ProducesResponseType(typeof(IEnumerable<TarefaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TarefaResponse>>> ListarTarefaPendentesAsync()
        {
            var tarefas = await _tarefaAppService.ListarTarefasPendentesAsync();
            return Ok(tarefas);
        }

        #endregion Public Methods
    }
}
