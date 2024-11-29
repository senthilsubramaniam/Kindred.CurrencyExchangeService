using Kindred.CurrencyExchangeService.Application;
using Kindred.CurrencyExchangeService.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kindred.CurrencyExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExchangeServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeServiceController(IMediator  mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CurrencyExchangeResponse>> Post([FromBody] ExchangeRateServiceCommand request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An unexpected error occurred.",
                    Details = ex.Message
                });
            }
        }
    }
}
