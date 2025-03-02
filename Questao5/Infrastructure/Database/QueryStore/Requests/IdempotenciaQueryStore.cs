using Dapper;
using Newtonsoft.Json;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class IdempotenciaQueryStore : IIdempotenciaQueryStore
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaQueryStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ResultadoMovimentacao?> ObterResultado(string idempotencyKey)
        {
            var resultadoJson = await _dbConnection.QuerySingleOrDefaultAsync<string>(
                "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @IdempotencyKey",
                new { IdempotencyKey = idempotencyKey });

            return resultadoJson != null ? JsonConvert.DeserializeObject<ResultadoMovimentacao>(resultadoJson) : null;
        }
    }
}
