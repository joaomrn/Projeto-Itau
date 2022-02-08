using Confluent.Kafka;
using Kafka.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Kafka.Domain.Services
{
    public class ConsumidorSchema : ConsumidorAbstrato, IConsumidorSchema
    {
        #region [ Propiedades ]

        private readonly ILogger<ConsumidorSchema> _logger;

        #endregion

        #region [ Consumidor ]

        public ConsumidorSchema(ILogger<ConsumidorSchema> logger) : base() { _logger = logger; }

        #endregion

        #region [ Métodos Publicos ]

        public ConsumeResult<TKey, TValue> Consumir<TKey, TValue>(string topico)
        {
            ConsumerConfig.GroupId = ConsumerGroup;

            using var consumer = new ConsumerBuilder<TKey, TValue>(ConsumerConfig).Build();
            try
            {
                consumer.Subscribe(topico);

                var cr = consumer.Consume();
                consumer.Commit(cr);

                _logger.LogInformation($"Consumido registro da partição {cr.Partition}");
                _logger.LogInformation($"Consumido offset {cr.Offset}");
                _logger.LogInformation($"Consumido chave '{cr.Message.Key}'");
                _logger.LogInformation($"Consumido valor '{cr.Message.Value}'");                

                return cr;
            }
            finally
            {
                consumer.Close();
            }

        }

        #endregion
    }
}
