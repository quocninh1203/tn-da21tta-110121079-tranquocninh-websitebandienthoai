using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Rams;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class RamAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public RamAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateRam")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateRamRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateRam")]
        public async Task<ActionResult<CommandResult>> Update(int ramId, [FromBody] UpdateRamRequest newRam)
        {
            var request = new UpdateRamRequest()
            {
                RamId = ramId,
                Size = newRam.Size,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllRams")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllRamRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteRam")]
        public async Task<ActionResult<CommandResult>> Remove(int ramId)
        {
            var request = new DeleteRamRequest()
            {
                RamId = ramId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
