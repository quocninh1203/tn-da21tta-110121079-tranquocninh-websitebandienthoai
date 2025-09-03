using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Requests.Brands;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.API.Endpoints
{
    [Route("api/[Controller]")] 
    [ApiController]
    public class BrandAPI : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandAPI(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateBrand")]
        public async Task<ActionResult<CommandResult>> Create([FromBody] CreateBrandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut("UpdateBrand")]
        public async Task<ActionResult<CommandResult>> Update(int brandId, [FromBody] UpdateBrandRequest newBrand)
        {
            var request = new UpdateBrandRequest()
            {
                BrandId = brandId,
                Name = newBrand.Name,
                Slug = newBrand.Slug,
                ImageBase64 = newBrand.ImageBase64
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("GetBrand")]
        public async Task<ActionResult<QueryResult<object>>> GetById(int brandId)
        {
            var request = new GetDetailBrandRequest()
            {
                BrandId = brandId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        //[Authorize]   
        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<QueryResult<List<object>>>> GetAll()
        {
            var request = new GetAllBrandRequest();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("DeleteBrand")]
        public async Task<ActionResult<CommandResult>> Remove(int brandId)
        {
            var request = new DeleteBrandRequest()
            {
                BrandId = brandId
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
