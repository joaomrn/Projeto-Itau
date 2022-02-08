using Confluent.Kafka;
using Kafka.Core.Entities;
using Kafka.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kafka.Domain.Services.Produtor
{
    public class ProdutorSchema : IProdutorSchema
    {
        #region [ Propiedades ]

        private readonly ILogger<ProdutorSchema> _logger;

        #endregion

        #region [ Construtor ]

        public ProdutorSchema(ILogger<ProdutorSchema> logger)
        {
            _logger = logger;
        }

        #endregion

        #region [ Métodos Publicos ]

        public async Task<bool> Enviar(string topico, int key, string propostaAverbacao)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAPSERVERS")
            };

            using (var producer = new ProducerBuilder<int, string>(config).Build())
            {
                var result = await producer.ProduceAsync(topico, new Message<int, string>() { Key = key, Value = propostaAverbacao });

                _logger.LogInformation($"Entregou a mensagem com key '{result.Message.Key}', value '{result.Message.Value}' na partição '{result.Partition}' e offset '{result.Offset}'");

                if (result != null) return true;
                else return false;
            }
        }

        #endregion
    }
}
