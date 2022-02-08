using Confluent.Kafka;

namespace Kafka.Domain.Interfaces
{
    public interface IConsumidorSchema
    {
        ConsumeResult<TKey, TValue> Consumir<TKey, TValue>(string topico);
    }
}
