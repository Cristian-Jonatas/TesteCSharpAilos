using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces
{
    public interface IIdempotenciaCommandStore
    {
        Task SalvarResultado(string idempotencyKey, MovimentarContaCommand request, ResultadoMovimentacao resultado);
    }
}
