using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Phones;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneAPI : ControllerBase
    {
        private readonly IMediator _mediator;   

        public PhoneAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreatePhone")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreatePhoneRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdatePhone")]
        public async Task<ActionResult<CommandResult>> Update(int phoneId, [FromBody] UpdatePhoneRequest newPhone)
        {
            var request = new UpdatePhoneRequest
            {
                PhoneId = phoneId,
                Name = newPhone.Name,
                BrandId = newPhone.BrandId,
                ImageBase64 = newPhone.ImageBase64,
                Screen = newPhone.Screen,
                Chip = newPhone.Chip,
                Battery = newPhone.Battery,
                Description = newPhone.Description,
                Slug = newPhone.Slug,
                IsActive = newPhone.IsActive
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetPhone")]
        public async Task<ActionResult<QueryResult<object>>> GetById(int phoneId)
        {
            var request = new GetDetailPhoneRequest
            {
                PhoneId = phoneId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllPhones")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllPhoneRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetPhoneActivate")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetActivate()
        {
            var request = new GetPhoneActivateRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("ToggleActive")]
        public async Task<ActionResult<CommandResult>> Toggle(int phoneId)
        {
            var request = new ActivatePhoneRequest()
            {
                PhoneId = phoneId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeletePhone")]
        public async Task<ActionResult<CommandResult>> Delete(int phoneId)
        {
            var request = new DeletePhoneRequest
            {
                PhoneId = phoneId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("RecommendProducts")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetRecommendedProducts(int userId)
        {
            var request = new RecommendRequest
            {
                UserId = userId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
