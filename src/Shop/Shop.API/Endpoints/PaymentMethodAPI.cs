using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.PaymentMethods;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PaymentMethodAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentMethodAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreatePaymentMethod")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreatePaymentMethodRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdatePaymentMethod")]
        public async Task<ActionResult<CommandResult>> Update(int methodId, [FromBody] UpdatePaymentMethodRequest newPaymentMethod)
        {
            var request = new UpdatePaymentMethodRequest()
            {
                PayMethodId = methodId,
                Name = newPaymentMethod.Name,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllPaymentMethods")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllPaymentMethodRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeletePaymentMethod")]
        public async Task<ActionResult<CommandResult>> Remove(int methodId)
        {
            var request = new DeletePaymentMethodRequest()
            {
                PayMethodId = methodId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
