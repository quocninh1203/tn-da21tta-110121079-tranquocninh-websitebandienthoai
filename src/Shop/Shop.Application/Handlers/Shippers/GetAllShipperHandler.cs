using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Shippers;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Shippers
{
    public class GetAllShipperHandler : IRequestHandler<GetAllShipperRequest, QueryResult<List<Shipper>>>
    {
        private readonly IShipperRepository _shipperRepository;
        public GetAllShipperHandler(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }
        public async Task<QueryResult<List<Shipper>>> Handle(GetAllShipperRequest request, CancellationToken cancellationToken)
        {
            var ships = await _shipperRepository.GetAsync();
            var response = new QueryResult<List<Shipper>>();

            if (ships == null || !ships.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Shipper));
                response.Code = StatusCode.NotFound;
                response.Model = new List<Shipper>();
                return response;
            }
            response.Model = (List<Shipper>)ships;

            return response;
        }
    }
}
