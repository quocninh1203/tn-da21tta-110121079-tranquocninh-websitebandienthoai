using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Rams;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Rams
{
    public class GetAllRamHandler : IRequestHandler<GetAllRamRequest, QueryResult<List<Ram>>>
    {
        private readonly IRamRepository _ramRepository;
        public GetAllRamHandler(IRamRepository ramRepository)
        {
            _ramRepository = ramRepository;
        }
        public async Task<QueryResult<List<Ram>>> Handle(GetAllRamRequest request, CancellationToken cancellationToken)
        {
            var rams = await _ramRepository.GetAsync();
            var response = new QueryResult<List<Ram>>();

            if (rams == null || !rams.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Ram));
                response.Code = StatusCode.NotFound;
                response.Model = new List<Ram>();
                return response;
            }
            response.Model = (List<Ram>)rams;

            return response;
        }
    }
}
