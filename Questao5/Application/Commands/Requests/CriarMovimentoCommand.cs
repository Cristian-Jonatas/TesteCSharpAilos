namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentoCommand
    {
        public string? IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string? DataMovimento { get; set; }
        public char TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
        public decimal Valor { get; set; }
    }
}
