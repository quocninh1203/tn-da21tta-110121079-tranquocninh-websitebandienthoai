using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Shippers;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Shippers
{
    public class DeleteShipperHandler : IRequestHandler<DeleteShipperRequest, CommandResult>
    {
        private readonly IShipperRepository _shipperRepository;
        private readonly IOrderRepository _orderRepository;
        public DeleteShipperHandler(IShipperRepository shipperRepository, IOrderRepository orderRepository)
        {
            _shipperRepository = shipperRepository;
            _orderRepository = orderRepository;
        }
        public async Task<CommandResult> Handle(DeleteShipperRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var order = await _orderRepository.GetAsync(p => p.ShipperId == request.ShipId);
            if (order.Count > 0)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.CannotDelete, nameof(Shipper));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var ship = await _shipperRepository.GetByIdAsync(request.ShipId);
            if (ship is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Shipper));
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _shipperRepository.Delete(ship);

            return result;
        }
    }
}
