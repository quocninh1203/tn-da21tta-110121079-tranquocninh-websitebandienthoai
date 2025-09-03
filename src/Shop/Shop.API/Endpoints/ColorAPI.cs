using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Colors;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ColorAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public ColorAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateColor")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateColorRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateColor")]
        public async Task<ActionResult<CommandResult>> Update(int colorId, [FromBody] UpdateColorRequest newColor)
        {
            var request = new UpdateColorRequest()
            {
                ColorId = colorId,
                Name = newColor.Name,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllColors")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllColorRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteColor")]
        public async Task<ActionResult<CommandResult>> Remove(int colorId)
        {
            var request = new DeleteColorRequest()
            {
                ColorId = colorId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
