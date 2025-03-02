using Dapper;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class IdempotenciaCommandStore: IIdempotenciaCommandStore
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaCommandStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task SalvarResultado(string idempotencyKey, MovimentarContaCommand request, ResultadoMovimentacao resultado)
        {
            var requisicaoJson = JsonConvert.SerializeObject(request);
            var resultadoJson = JsonConvert.SerializeObject(resultado);

            await _dbConnection.ExecuteAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@IdempotencyKey, @Requisicao, @Resultado)",
                new { IdempotencyKey = idempotencyKey, Requisicao = requisicaoJson, Resultado = resultadoJson });
        }
    }
}
