using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.PhoneVariants;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneVariantAPI : ControllerBase
    {
        private readonly IMediator _mediator;

        public PhoneVariantAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreatePhoneVariant")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreatePhoneVariantRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdatePhoneVariant")]
        public async Task<ActionResult<CommandResult>> Update(int variantId, [FromBody] UpdatePhoneVariantRequest newVariant)
        {
            var request = new UpdatePhoneVariantRequest
            {
                VariantId = variantId,
                PhoneId = newVariant.PhoneId,
                ColorId = newVariant.ColorId,
                RamId = newVariant.RamId,
                StorageId = newVariant.StorageId,
                Price = newVariant.Price,
                StockQuantity = newVariant.StockQuantity,
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
