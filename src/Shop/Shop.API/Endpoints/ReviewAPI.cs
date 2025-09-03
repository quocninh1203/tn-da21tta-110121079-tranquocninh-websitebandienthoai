using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Reviews;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ReviewAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateReview")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateReviewRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
