using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, ResultadoMovimentacao>
    {
        private readonly IContaCorrenteStore _contaCorrenteStore;
        private readonly IMovimentarContaCommandStore _movimentarContaCommandStore;
        private readonly IIdempotenciaCommandStore _idempotenciaCommandStore;
        private readonly IIdempotenciaQueryStore _idempotenciaQueryStore;

        public MovimentarContaHandler(IContaCorrenteStore contaCorrenteStore, IMovimentarContaCommandStore movimentarContaCommandStore, IIdempotenciaCommandStore idempotenciaCommandStore, IIdempotenciaQueryStore idempotenciaQueryStore)
        {
            _contaCorrenteStore = contaCorrenteStore;
            _movimentarContaCommandStore = movimentarContaCommandStore;
            _idempotenciaCommandStore = idempotenciaCommandStore;
            _idempotenciaQueryStore = idempotenciaQueryStore;
        }

        public async Task<ResultadoMovimentacao> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            var resultadoExistente = await _idempotenciaQueryStore.ObterResultado(request.IdempotencyKey);
            if (resultadoExistente != null)
            {
                return resultadoExistente;
            }

            var conta = await _contaCorrenteStore.ObterConta(request.command.IdContaCorrente.ToString());

            if (conta == null) throw new BadHttpRequestException(MensagemErro.INVALID_ACCOUNT.GetDescription());
            if (conta.Ativo == 0) throw new BadHttpRequestException(MensagemErro.INACTIVE_ACCOUNT.GetDescription());
            if (request.command.Valor <= 0) throw new BadHttpRequestException(MensagemErro.INVALID_VALUE.GetDescription());
            if (Char.ToUpper(request.command.TipoMovimento) != 'C' && Char.ToUpper(request.command.TipoMovimento) != 'D') throw new BadHttpRequestException(MensagemErro.INVALID_TYPE.GetDescription());

            var idMovimento = Guid.NewGuid().ToString();
            var movimento = new CriarMovimentoCommand()
            {
                IdMovimento = idMovimento,
                IdContaCorrente = request.command.IdContaCorrente,
                DataMovimento = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                TipoMovimento = Char.ToUpper(request.command.TipoMovimento),
                Valor = request.command.Valor
            };

            await _movimentarContaCommandStore.InserirMovimentacao(movimento);
            var resultado = new ResultadoMovimentacao { IdMovimento = idMovimento };

            await _idempotenciaCommandStore.SalvarResultado(request.IdempotencyKey, request, resultado);

            return resultado;
            
        }
    }
}
