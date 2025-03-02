using FluentAssertions.Equivalency;

namespace Questao5.Domain.Entities
{
    public class ResultadoSaldo
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Saldo { get; set; }
    }
}
