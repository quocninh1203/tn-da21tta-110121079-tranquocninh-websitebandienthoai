using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Carts;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CartAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateCart")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateCartRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        //[Authorize(Roles = "1")]
        [HttpPut("UpdateQuantityCart")]
        public async Task<ActionResult<CommandResult>> Update(int cartId, [FromBody] UpdateCartRequest newCart)
        {
            var request = new UpdateCartRequest()
            {
                CartId = cartId,
                Quantity = newCart.Quantity,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        //[Authorize(Roles = "1")]
        [HttpGet("GetAllCartByUserId")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll(int userId)
        {
            var request = new GetAllCartByUserIdRequest()
            {
                UserId = userId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteCart")]
        public async Task<ActionResult<CommandResult>> Remove(int cartId)
        {
            var request = new DeleteCartRequest()
            {
                CartId = cartId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
