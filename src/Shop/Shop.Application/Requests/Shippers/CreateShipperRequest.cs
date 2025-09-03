using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Shippers
{
    public class CreateShipperRequest : IRequest<CommandResult>
    {
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
