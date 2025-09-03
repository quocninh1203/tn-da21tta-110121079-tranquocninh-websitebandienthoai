using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Orders;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateOrderRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllOrderRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetOrderByUserId")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetOrdersByUserId(int userId)
        {
            var request = new GetOrdersByUserIdRequest()
            {
                UserId = userId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetOrderDetailByOrderId")]
        public async Task<ActionResult<QueryResult<object>>> GetOrderDetailByOrderId(int orderId)
        {
            var request = new GetOrderDetailByOrderIdRequest()
            {
                OrderId = orderId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("ChangeStatus")]
        public async Task<ActionResult<CommandResult>> Update([FromBody] ChangeStatusRequest order, int orderId)
        {
            var request = new ChangeStatusRequest()
            {
                OrderId = orderId,
                Status = order.Status
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<ActionResult<CommandResult>> Delete(int orderId)
        {
            var request = new DeleteOrderRequest()
            {
                OrderId = orderId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
