using Questao5.Application.Commands.Requests;

namespace Questao5.Application.Interfaces
{
    public interface IMovimentarContaCommandStore
    {
        Task InserirMovimentacao(CriarMovimentoCommand conta);
    }
}
