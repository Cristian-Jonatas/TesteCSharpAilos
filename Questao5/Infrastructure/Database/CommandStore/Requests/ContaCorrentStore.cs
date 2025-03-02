using Dapper;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class ContaCorrenteStore: IContaCorrenteStore
    {
        private readonly IDbConnection _dbConnection;

        public ContaCorrenteStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ContaCorrente> ObterConta(string idContaCorrente)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = idContaCorrente });
        }
    }
}
