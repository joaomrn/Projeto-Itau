using Confluent.Kafka;
using Kafka.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Kafka.Domain.Interfaces
{
    public interface IProdutorSchema
    {
        Task<bool> Enviar(int key, PropostaAverbacao propostaAverbacao);
    }
}
