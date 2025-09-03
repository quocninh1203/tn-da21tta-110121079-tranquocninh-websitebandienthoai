using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Shippers;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Shippers
{
    public class CreateShipperHandler : IRequestHandler<CreateShipperRequest, CommandResult>
    {
        private readonly IShipperRepository _shipperRepository;
        public CreateShipperHandler(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }
        public async Task<CommandResult> Handle(CreateShipperRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _shipperRepository.GetSingleAsync(r => r.Name == request.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Shipper));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var ship = new Shipper
            {
                Name= request.Name,
                Cost = request.Cost,
            };
            await _shipperRepository.Add(ship);

            return result;
        }
    }
}
