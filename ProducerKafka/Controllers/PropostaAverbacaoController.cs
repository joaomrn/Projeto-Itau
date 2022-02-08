using Kafka.Core.Entities;
using Kafka.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ProducerKafka.Controllers
{
    [ApiController]
    [Route("api")]
    public class PropostaAverbacaoController : ControllerBase
    {
        #region [ Propiedades ]

        private readonly ILogger<PropostaAverbacaoController> _logger;
        private readonly IProdutorSchema _produtorSchema;
        private string _parcelaInvalida;
        private string _prazoContratoInvalida;
        private string _dataVencimentoInvalida;

        #endregion

        #region [ Construtor ]

        public PropostaAverbacaoController(ILogger<PropostaAverbacaoController> logger, IProdutorSchema produtorSchema)
        {
            _logger = logger;
            _produtorSchema = produtorSchema;
        }

        #endregion

        #region [ Métodos Publicos ]

        [HttpPost("propostaaverbacao")]
        public async Task<IActionResult> PropostaAverbacao(PropostaAverbacao propostaAverbacao)
        {
            try
            {
                var key = new Random().Next(0, 100000);

                ValidaRequisicao(propostaAverbacao);

                if (!string.IsNullOrEmpty(_parcelaInvalida) ||
                    !string.IsNullOrEmpty(_prazoContratoInvalida) ||
                    !string.IsNullOrEmpty(_dataVencimentoInvalida))
                {
                    return BadRequest(
                        new
                        {
                            parcelaInvalida = _parcelaInvalida,
                            prazoContratoInvalida = _prazoContratoInvalida,
                            dataVencimentoInvalida = _dataVencimentoInvalida
                        }
                        );
                }

                var resultado = Newtonsoft.Json.JsonConvert.SerializeObject(propostaAverbacao);

                var result = await _produtorSchema.Enviar(Environment.GetEnvironmentVariable("TOPICOAVERBACAO"), key, resultado);

                if (result) return Ok();
                else return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex);
                return NotFound();
            }
        }

        #endregion

        #region [ Métodos Privados ]

        private void ValidaRequisicao(PropostaAverbacao propostaAverbacao)
        {
            if (propostaAverbacao.ValorParcela <= 0)
            {
                _parcelaInvalida = "Valor da parcela deve ser valido.";
            }

            if (propostaAverbacao.PrazoContrato <= 0)
            {
                _prazoContratoInvalida = "Deve ser informado um valor valido para o prazo do contrato.";
            }

            if (propostaAverbacao.DataPrimeiroVencimento < DateTime.Now)
            {
                _dataVencimentoInvalida = "Informar uma data de vencimento valida";
            }
        }

        #endregion
    }
}
