using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Kafka.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Domain.Services
{
    public class ConsumidorAbstrato
    {
        private BrokerHelper _brokerHelper = null;
        protected readonly string ConsumerGroup;

        protected ConsumidorAbstrato()
        {
            _brokerHelper = new BrokerHelper(Environment.GetEnvironmentVariable("BootstrapServers"),
                                           Environment.GetEnvironmentVariable("SecurityProtocol"),
                                           Environment.GetEnvironmentVariable("SaslMechanism"),
                                           Environment.GetEnvironmentVariable("SaslUsername"),
                                           Environment.GetEnvironmentVariable("SaslPassword"),
                                           Environment.GetEnvironmentVariable("SslCaLocation"),
                                           Environment.GetEnvironmentVariable("SchemaRegistryUrl"),
                                           int.Parse(Environment.GetEnvironmentVariable("SchemaRegistryRequestTimeoutMs")),
                                           int.Parse(Environment.GetEnvironmentVariable("SchemaRegistryMaxCachedSchemas")),
                                           Environment.GetEnvironmentVariable("SchemaRegistryBasicAuthUserInfo"));

            ConsumerGroup = Environment.GetEnvironmentVariable("NomeConsumerGroup");
        }

        public ConsumerConfig ConsumerConfig
        {
            get => _brokerHelper.ConsumerConfig;
            set => _brokerHelper.ConsumerConfig = value;
        }

        public SchemaRegistryConfig SchemaRegistryConfig
        {
            get => _brokerHelper.SchemaRegistryConfig;
        }

        public virtual void Unsubscribe<TKey, TValue>(string ConsumerGroup)
        {
            _brokerHelper.ConsumerConfig.GroupId = ConsumerGroup;
            using (var consumer = new ConsumerBuilder<TKey, TValue>(_brokerHelper.ConsumerConfig).Build())
            {
                consumer.Unsubscribe();
                consumer.Close();
            }
        }
    }
}
