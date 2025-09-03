using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Storages;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Storages
{
    public class GetAllStorageHandler : IRequestHandler<GetAllStorageRequest, QueryResult<List<Storage>>>
    {
        private readonly IStorageRepository _storageRepository;
        public GetAllStorageHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        public async Task<QueryResult<List<Storage>>> Handle(GetAllStorageRequest request, CancellationToken cancellationToken)
        {
            var storages = await _storageRepository.GetAsync();
            var response = new QueryResult<List<Storage>>();

            if (storages == null || !storages.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Storage));
                response.Code = StatusCode.NotFound;
                response.Model = new List<Storage>();
            }
            response.Model = (List<Storage>)storages;

            return response;
        }
    }
}
