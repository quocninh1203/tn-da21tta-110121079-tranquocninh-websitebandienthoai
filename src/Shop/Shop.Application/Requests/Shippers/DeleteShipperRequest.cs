using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Shippers
{
    public class DeleteShipperRequest : IRequest<CommandResult>
    {
        public int ShipId { get; set; }
    }
}
