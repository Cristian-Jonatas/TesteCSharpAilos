using Microsoft.AspNetCore.Http;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Tests;

public class MovimentarContaTests
{
    private readonly Mock<IContaCorrenteStore> _mockContaCorrenteStore;
    private readonly Mock<IMovimentarContaCommandStore> _mockMovimentarContaCommandStore;
    private readonly Mock<IIdempotenciaCommandStore> _mockIdempotenciaCommandStore;
    private readonly Mock<IIdempotenciaQueryStore> _mockIdempotenciaQueryStore;
    private readonly MovimentarContaHandler _handler;
    private readonly ContaCorrente _contaAtiva = ContaCorrenteMoqTeste.ContaAtiva;
    private readonly ContaCorrente _contaInativa = ContaCorrenteMoqTeste.ContaInativa;

    public MovimentarContaTests()
    {
        _mockContaCorrenteStore = new Mock<IContaCorrenteStore>();
        _mockMovimentarContaCommandStore = new Mock<IMovimentarContaCommandStore>();
        _mockIdempotenciaCommandStore = new Mock<IIdempotenciaCommandStore>();
        _mockIdempotenciaQueryStore = new Mock<IIdempotenciaQueryStore>();

        _handler = new MovimentarContaHandler(_mockContaCorrenteStore.Object, _mockMovimentarContaCommandStore.Object, _mockIdempotenciaCommandStore.Object, _mockIdempotenciaQueryStore.Object);
    }

    [Fact]
    public async Task MovimentarConta_DeveRetornarBadRequest_ContaInexistente()
    {
        var command = new CriarMovimentoCommand { IdContaCorrente = "999999", TipoMovimento = 'C', Valor = 100 };

        _mockContaCorrenteStore.Setup(x => x.ObterConta(command.IdContaCorrente)).ReturnsAsync((ContaCorrente)null);

        var request = new MovimentarContaCommand(command, "idempotency-key");

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INVALID_ACCOUNT.GetDescription(), exception.Message);
    }

    [Fact]
    public async Task MovimentarConta_DeveRetornarBadRequest_ContaInativa()
    {
        var command = new CriarMovimentoCommand { IdContaCorrente = _contaInativa.IdContaCorrente, TipoMovimento = 'C', Valor = 100 };

        _mockContaCorrenteStore.Setup(x => x.ObterConta(command.IdContaCorrente)).ReturnsAsync(_contaInativa);

        var request = new MovimentarContaCommand(command, "idempotency-key");

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INACTIVE_ACCOUNT.GetDescription(), exception.Message);
    }

    [Fact]
    public async Task MovimentarConta_DeveRetornarBadRequest_ValorInvalido()
    {
        var command = new CriarMovimentoCommand { IdContaCorrente = _contaAtiva.IdContaCorrente, TipoMovimento = 'C', Valor = 0 };

        _mockContaCorrenteStore.Setup(x => x.ObterConta(command.IdContaCorrente)).ReturnsAsync(_contaAtiva);

        var request = new MovimentarContaCommand(command, "idempotency-key");

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INVALID_VALUE.GetDescription(), exception.Message);
    }

    [Fact]
    public async Task MovimentarConta_DeveRetornarBadRequest_TipoMovimentoInvalido()
    {
        var command = new CriarMovimentoCommand { IdContaCorrente = _contaAtiva.IdContaCorrente, TipoMovimento = 'X', Valor = 100 };

        _mockContaCorrenteStore.Setup(x => x.ObterConta(command.IdContaCorrente)).ReturnsAsync(_contaAtiva);

        var request = new MovimentarContaCommand(command, "idempotency-key");

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INVALID_TYPE.GetDescription(), exception.Message);
    }

    [Fact]
    public async Task MovimentarConta_DeveRetornarSucesso()
    {
        var command = new CriarMovimentoCommand { IdContaCorrente = _contaAtiva.IdContaCorrente, TipoMovimento = 'C', Valor = 100 };

        _mockContaCorrenteStore.Setup(x => x.ObterConta(command.IdContaCorrente)).ReturnsAsync(_contaAtiva);

        _mockIdempotenciaQueryStore.Setup(x => x.ObterResultado(It.IsAny<string>())).ReturnsAsync((ResultadoMovimentacao)null);

        _mockMovimentarContaCommandStore.Setup(x => x.InserirMovimentacao(It.IsAny<CriarMovimentoCommand>())).Returns(Task.CompletedTask);

        _mockIdempotenciaCommandStore.Setup(x => x.SalvarResultado(It.IsAny<string>(), It.IsAny<MovimentarContaCommand>(), It.IsAny<ResultadoMovimentacao>())).Returns(Task.CompletedTask);

        var request = new MovimentarContaCommand(command, "idempotency-key");

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.IdMovimento));
    }
}
