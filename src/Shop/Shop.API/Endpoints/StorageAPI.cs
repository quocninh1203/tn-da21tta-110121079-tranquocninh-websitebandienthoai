using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Storages;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class StorageAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public StorageAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateStorage")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateStorageRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateStorage")]
        public async Task<ActionResult<CommandResult>> Update(int storageId, [FromBody] UpdateStorageRequest newStorage)
        {
            var request = new UpdateStorageRequest()
            {
                StorageId = storageId,
                Size = newStorage.Size,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllStorages")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllStorageRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteStorage")]
        public async Task<ActionResult<CommandResult>> Remove(int storageId)
        {
            var request = new DeleteStorageRequest()
            {
                StorageId = storageId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
