using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria
    {
        public int Numero { get; }
        public string Titular { get; private set; }
        private double Saldo;
        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular, double depositoInicial = 0.0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                Saldo += valor;
            }
            else
            {
                Console.WriteLine("Valor de depósito inválido.");
            }
        }

        public void Saque(double valor)
        {
            if (valor > 0)
            {
                Saldo -= (valor + TaxaSaque);
            }
            else
            {
                Console.WriteLine("Valor de saque inválido.");
            }
        }

        public void AlterarTitular(string novoTitular)//Método criado caso precise usar em algum momento
        {
            if (!string.IsNullOrWhiteSpace(novoTitular))
            {
                Titular = novoTitular;
            }
            else
            {
                Console.WriteLine("Nome inválido.");
            }
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }

    }
}
