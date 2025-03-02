namespace Questao5.Application.Interfaces
{
    public interface IConsultarSaldoQueryStore
    {
        Task<decimal> ObterSaldo(string idContaCorrente);
    }
}
