using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] CriarMovimentoCommand command, [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey)
        {
            try
            {
                var resultado = await _mediator.Send(new MovimentarContaCommand(command, idempotencyKey));
                return Ok(resultado.IdMovimento);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("saldo/{idContaCorrente}")]
        public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
        {
            try
            {
                var resultado = await _mediator.Send(new ConsultarSaldoQuery(idContaCorrente));
                return Ok(resultado);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}