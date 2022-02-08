using Kafka.Core.Entities;
using System.Threading.Tasks;

namespace Kafka.Domain.Interfaces
{
    public interface IProdutorSchema
    {
        Task<bool> Enviar(string topico, int key, string propostaAverbacao);
    }
}
