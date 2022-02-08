using Confluent.Kafka;
using Kafka.Core.Entities;
using Kafka.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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

        public async Task<bool> Enviar(int key, PropostaAverbacao propostaAverbacao)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAPSERVERS")
            };

            var resultado = Newtonsoft.Json.JsonConvert.SerializeObject(propostaAverbacao);

            using (var producer = new ProducerBuilder<int, string>(config).Build())
            {
                var result = await producer.ProduceAsync(
                    Environment.GetEnvironmentVariable("TOPICOAVERBACAO"),
                    new Message<int, string>() { Key = key, Value = resultado });

                _logger.LogInformation($"Entregou a mensagem com key '{result.Message.Key}', value '{result.Message.Value}' na partição '{result.Partition}' e offset '{result.Offset}'");

                if (result != null) return true;
                else return false;
            }
        }

        #endregion
    }
}
