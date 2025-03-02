using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Responses
{
    public record ConsultarSaldoQuery(string IdContaCorrente) : IRequest<ResultadoSaldo>;
}
