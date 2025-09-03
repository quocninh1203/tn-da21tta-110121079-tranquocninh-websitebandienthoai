using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.OrderDetails;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderDetailAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderDetailAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateOrderDetail")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateOrderDetailRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
