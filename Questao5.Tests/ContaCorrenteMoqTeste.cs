using Questao5.Domain.Entities;

namespace Questao5.Tests
{
    public class ContaCorrenteMoqTeste
    {
        public static ContaCorrente ContaAtiva => new ContaCorrente
        {
            IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
            Numero = 123,
            Nome = "Katherine Sanchez",
            Ativo = 1
        };

        public static ContaCorrente ContaInativa => new ContaCorrente
        {
            IdContaCorrente = "D2E02051-7067-ED11-94C0-835DFA4A16C9",
            Numero = 963,
            Nome = "Elisha Simons",
            Ativo = 0
        };
    }
}
