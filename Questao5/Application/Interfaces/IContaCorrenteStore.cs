using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces
{
    public interface IContaCorrenteStore
    {
        Task<ContaCorrente> ObterConta(string idContaCorrente);
    }
}
