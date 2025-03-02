using Dapper;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class MovimentarContaCommandStore: IMovimentarContaCommandStore
    {
        private readonly IDbConnection _dbConnection;

        public MovimentarContaCommandStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task InserirMovimentacao(CriarMovimentoCommand conta)
        {
            await _dbConnection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                new { IdMovimento = conta.IdMovimento, IdContaCorrente = conta.IdContaCorrente, DataMovimento = conta.DataMovimento, TipoMovimento = conta.TipoMovimento, Valor = conta.Valor });
        }
    }
}
