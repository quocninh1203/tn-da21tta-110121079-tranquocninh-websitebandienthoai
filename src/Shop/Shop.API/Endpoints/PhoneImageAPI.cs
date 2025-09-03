using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.PhoneImages;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneImageAPI : ControllerBase
    {
        private readonly IMediator _mediator;

        public PhoneImageAPI(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("CreatePhoneImage")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreatePhoneImageRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdatePhoneImage")]
        public async Task<ActionResult<CommandResult>> Update(int phoneImageId, [FromBody] UpdatePhoneImageRequest newImage)
        {
            var request = new UpdatePhoneImageRequest()
            {
                ImgId = phoneImageId,
                PhoneId = newImage.PhoneId,
                ImageBase64 = newImage.ImageBase64,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeletePhoneImage")]
        public async Task<ActionResult<CommandResult>> Remove(int phoneImageId)
        {
            var request = new DeletePhoneImageRequest()
            {
                ImgId = phoneImageId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
