using MediatR;
using Questao5.Domain.Entities;
namespace Questao5.Application.Commands.Requests
{
    public record MovimentarContaCommand(CriarMovimentoCommand command, string IdempotencyKey) : IRequest<ResultadoMovimentacao>;

}
