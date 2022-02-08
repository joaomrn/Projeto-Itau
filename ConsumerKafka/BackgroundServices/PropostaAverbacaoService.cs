using Confluent.Kafka;
using Kafka.Core.Entities;
using Kafka.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerKafka.BackgroundServices
{
    public class PropostaAverbacaoService : BackgroundService
    {
        #region [ Propriedades ]

        private readonly IConsumidorSchema _consumidorSchema;
        private readonly ILogger<PropostaAverbacaoService> _logger;

        #endregion

        #region [ Consumidor ]

        public PropostaAverbacaoService(IConsumidorSchema consumidorSchema, ILogger<PropostaAverbacaoService> logger)
        {
            _consumidorSchema = consumidorSchema;
            _logger = logger;
        }

        #endregion

        #region [ Métodos Publicos ]

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Consumir();
            }
        }

        #endregion

        #region [ Métodos Privados ]

        private async Task Consumir()
        {
            var resposta = await Task.Factory.StartNew(() => _consumidorSchema.Consumir<int, string>(Environment.GetEnvironmentVariable("TOPICOAVERBACAO")));

            var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<PropostaAverbacao>(resposta.Message.Value);

            _logger.LogInformation($"Envio de requisição para API do INSS Key: {resposta.Message.Key}, Value: {resposta.Message.Value}");
        }

        #endregion
    }
}
