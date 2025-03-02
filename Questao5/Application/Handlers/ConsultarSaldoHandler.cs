using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, ResultadoSaldo>
    {
        private readonly IContaCorrenteStore _contaCorrenteStore;
        private readonly IConsultarSaldoQueryStore _consultarSaldoQueryStore;

        public ConsultarSaldoHandler(IContaCorrenteStore contaCorrenteStore, IConsultarSaldoQueryStore consultarSaldoQueryStore)
        {
            _contaCorrenteStore = contaCorrenteStore;
            _consultarSaldoQueryStore = consultarSaldoQueryStore;
        }

        public async Task<ResultadoSaldo> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _contaCorrenteStore.ObterConta(request.IdContaCorrente);

            if (conta == null) throw new BadHttpRequestException(MensagemErro.INVALID_ACCOUNT.GetDescription()); 
            if (conta.Ativo == 0) throw new BadHttpRequestException(MensagemErro.INACTIVE_ACCOUNT.GetDescription()); 

            var saldo = await _consultarSaldoQueryStore.ObterSaldo(request.IdContaCorrente);

            return new ResultadoSaldo
            {
                NumeroConta = Convert.ToInt32(conta.Numero),
                NomeTitular = Convert.ToString(conta.Nome),
                Saldo = Convert.ToDecimal(saldo),
                DataHora = DateTime.UtcNow
            };
        }
    }
}
