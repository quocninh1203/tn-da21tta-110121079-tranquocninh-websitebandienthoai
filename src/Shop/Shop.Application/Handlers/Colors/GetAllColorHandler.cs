using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Colors;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Colors
{
    public class GetAllColorHandler : IRequestHandler<GetAllColorRequest, QueryResult<List<Color>>>
    {
        private readonly IColorRepository _colorRepository;
        public GetAllColorHandler(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        public async Task<QueryResult<List<Color>>> Handle(GetAllColorRequest request, CancellationToken cancellationToken)
        {
            var colors = await _colorRepository.GetAsync();
            var response = new QueryResult<List<Color>>();

            if (colors == null || !colors.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(Color)); 
                response.Code = StatusCode.NotFound;
                response.Model = new List<Color>();
            }
            response.Model = (List<Color>)colors;

            return response;
        }
    }
}
