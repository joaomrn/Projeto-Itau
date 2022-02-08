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
        private readonly IProdutorSchema _produtorSchema;
        private readonly ILogger<PropostaAverbacaoService> _logger;

        #endregion

        #region [ Consumidor ]

        public PropostaAverbacaoService(IConsumidorSchema consumidorSchema, IProdutorSchema produtorSchema, ILogger<PropostaAverbacaoService> logger)
        {
            _consumidorSchema = consumidorSchema;
            _logger = logger;
            _produtorSchema = produtorSchema;
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
            ConsumeResult<int, string> resposta = null;

            try
            {
                resposta = await Task.Factory.StartNew(() => _consumidorSchema.Consumir<int, string>(Environment.GetEnvironmentVariable("TOPICOAVERBACAO")));

                var resultado = Newtonsoft.Json.JsonConvert.DeserializeObject<PropostaAverbacao>(resposta.Message.Value);

                if (resultado == null)
                {
                    throw new Exception("resultado");
                }

                _logger.LogInformation($"Envio de requisição para API do INSS Key: {resposta.Message.Key}, Value: {resposta.Message.Value}");
            }
            catch (Exception ex)
            {
                if (resposta != null)
                {
                    await _produtorSchema.Enviar(Environment.GetEnvironmentVariable("TOPICOAVERBACAOFALHA"), resposta.Message.Key, resposta.Message.Value);
                }

                _logger.LogError(null, ex);
            }
        }

        #endregion
    }
}
