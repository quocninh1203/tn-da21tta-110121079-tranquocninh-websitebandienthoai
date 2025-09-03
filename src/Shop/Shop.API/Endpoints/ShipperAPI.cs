using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Rams;
using Shop.Application.Requests.Shippers;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ShipperAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public ShipperAPI(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CreateShipper")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateShipperRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateShipper")]
        public async Task<ActionResult<CommandResult>> Update(int shipId, [FromBody] UpdateShipperRequest newShip)
        {
            var request = new UpdateShipperRequest()
            {
                ShipId = shipId,
                Name = newShip.Name,
                Cost = newShip.Cost,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllShippers")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllShipperRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteShipper")]
        public async Task<ActionResult<CommandResult>> Remove(int shipId)
        {
            var request = new DeleteShipperRequest()
            {
                ShipId = shipId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
