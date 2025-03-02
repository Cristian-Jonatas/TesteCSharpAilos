using Dapper;
using Questao5.Application.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ConsultarSaldoQueryStore: IConsultarSaldoQueryStore
    {
        private readonly IDbConnection _dbConnection;

        public ConsultarSaldoQueryStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<decimal> ObterSaldo(string idContaCorrente)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(
                "SELECT COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END), 0) FROM movimento WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = idContaCorrente });
        }
    }
}
