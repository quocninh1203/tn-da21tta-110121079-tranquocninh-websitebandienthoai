using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Brands
{
    public class DeleteBrandRequest : IRequest<CommandResult>
    {
        public int BrandId { get; set; }
    }
}
