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
    }
}
