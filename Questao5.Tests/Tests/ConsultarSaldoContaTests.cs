using Microsoft.AspNetCore.Http;
using Moq;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Tests;

public class ConsultarSaldoContaTests
{
    private readonly ConsultarSaldoHandler _handler;
    private readonly Mock<IContaCorrenteStore> _mockContaCorrenteStore;
    private readonly Mock<IConsultarSaldoQueryStore> _mockConsultarSaldoQueryStore;
    private readonly ContaCorrente _contaAtiva = ContaCorrenteMoqTeste.ContaAtiva;
    private readonly ContaCorrente _contaInativa = ContaCorrenteMoqTeste.ContaInativa;

    public ConsultarSaldoContaTests()
    {
        _mockContaCorrenteStore = new Mock<IContaCorrenteStore>();
        _mockConsultarSaldoQueryStore = new Mock<IConsultarSaldoQueryStore>();
        _handler = new ConsultarSaldoHandler(_mockContaCorrenteStore.Object, _mockConsultarSaldoQueryStore.Object);
    }

    [Fact]
    public async Task ConsultarSaldo_DeveRetornarSucesso()
    {
        var idContaCorrente = _contaAtiva.IdContaCorrente;

        _mockContaCorrenteStore.Setup(x => x.ObterConta(idContaCorrente)).ReturnsAsync(_contaAtiva);

        var request = new ConsultarSaldoQuery(idContaCorrente);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
        Assert.False(result == null);
    }

    [Fact]
    public async Task ConsultarSaldo_DeveRetornarBadRequest_ContaInexistente()
    {
        var idContaCorrente = "99999999";

        _mockContaCorrenteStore.Setup(x => x.ObterConta(idContaCorrente)).ReturnsAsync((ContaCorrente)null);

        var request = new ConsultarSaldoQuery(idContaCorrente);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INVALID_ACCOUNT.GetDescription(), exception.Message);
    }

    [Fact]
    public async Task ConsultarSaldo_DeveRetornarBadRequest_ContaInativa()
    {
        var idContaCorrente = _contaInativa.IdContaCorrente;

        _mockContaCorrenteStore.Setup(x => x.ObterConta(idContaCorrente)).ReturnsAsync(_contaInativa);

        var request = new ConsultarSaldoQuery(idContaCorrente);

        var exception = await Assert.ThrowsAsync<BadHttpRequestException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Equal(MensagemErro.INACTIVE_ACCOUNT.GetDescription(), exception.Message);
    }

}
