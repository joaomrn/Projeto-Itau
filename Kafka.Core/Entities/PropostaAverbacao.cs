using System;

namespace Kafka.Core.Entities
{
    public class PropostaAverbacao
    {
        public double ValorParcela { get; set; }
        public int PrazoContrato { get; set; }
        public DateTime DataPrimeiroVencimento { get; set; }
    }
}
