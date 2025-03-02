using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum MensagemErro
    {
        [Description("Apenas contas correntes cadastradas podem consultar o saldo - TIPO: INVALID_ACCOUNT.")]
        INVALID_ACCOUNT,

        [Description("Apenas contas correntes ativas podem receber movimentação - TIPO: INACTIVE_ACCOUNT.")]
        INACTIVE_ACCOUNT,

        [Description("Apenas valores positivos podem ser recebidos - TIPO: INVALID_VALUE.")]
        INVALID_VALUE,

        [Description("Apenas os tipos D (débito) ou C (crédito) podem ser aceitos - TIPO: INVALID_TYPE.")]
        INVALID_TYPE
    }
}
