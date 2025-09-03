using MediatR;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Shippers
{
    public class UpdateShipperRequest : IRequest<CommandResult>
    {
        public int ShipId { get; set; }
        public string? Name { get; set; }
        public int? Cost { get; set; }
    }
}
