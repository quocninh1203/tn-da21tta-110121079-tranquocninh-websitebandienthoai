using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Users;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetProfileUser")]
        public async Task<ActionResult<QueryResult<object>>> GetById(int userId)
        {
            var request = new GetProfileRequest()
            {
                UserId = userId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<CommandResult>> Login([FromBody]LoginRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("Logout")]
        public async Task<ActionResult<CommandResult>> Logout(int userId)
        {
            var request = new LogoutRequest()
            {
                UserId = userId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllUserRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<CommandResult>> Register([FromBody] RegisterRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateProfile")]
        public async Task<ActionResult<CommandResult>> Update(int userId, [FromBody] UpdateProfileRequest newProfile)
        {
            var request = new UpdateProfileRequest()
            {
                UserId = userId,
                FullName = newProfile.FullName,
                PhoneNumber = newProfile.PhoneNumber,
                Address = newProfile.Address,
                ImageBase64 = newProfile.ImageBase64,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<CommandResult>> RefreshToken()
        {
            var request = new RefreshTokenRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("ChangePassWord")]
        public async Task<ActionResult<CommandResult>> ChangePW(int userId, [FromBody] ChangePassWordRequest changePassWord)
        {
            var request = new ChangePassWordRequest()
            {
                UserId = userId,
                PassWord = changePassWord.PassWord,
                NewPassWord = changePassWord.NewPassWord,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
