using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces
{
    public interface IIdempotenciaQueryStore
    {
        Task<ResultadoMovimentacao?> ObterResultado(string idempotencyKey);
    }
}
