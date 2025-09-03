using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.OTPs;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OtpAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public OtpAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Gửi mã OTP
        [HttpPost("SendOtp")]
        public async Task<ActionResult<CommandResult>> SendOtp([FromBody] SendOtpRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // Xác nhận mã OTP
        [HttpPost("ConfirmOtp")]
        public async Task<ActionResult<CommandResult>> ConfirmOtp([FromBody] ConfirmOtpRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
