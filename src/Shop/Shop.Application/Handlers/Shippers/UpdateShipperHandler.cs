using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Shippers;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Shippers
{
    public class UpdateShipperHandler : IRequestHandler<UpdateShipperRequest, CommandResult>
    {
        private readonly IShipperRepository _shipperRepository;
        public UpdateShipperHandler(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }
        public async Task<CommandResult> Handle(UpdateShipperRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var ship = await _shipperRepository.GetByIdAsync(request.ShipId);
            if (ship is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Shipper));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new Shipper
            {
                Name = request.Name,
                Cost = (int)request.Cost,
            };
            ship.UpdateWith(updateEntity);

            var check = await _shipperRepository.GetSingleAsync(r => r.Name == ship.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Shipper));
                result.Code = StatusCode.Conflict;
                return result;
            }

            await _shipperRepository.Update(ship);
            return result;
        }
    }
}
