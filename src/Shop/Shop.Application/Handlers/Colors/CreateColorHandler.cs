using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Colors;
using Shop.Domain.Entities;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Colors
{
    public class CreateColorHandler : IRequestHandler<CreateColorRequest, CommandResult>
    {
        private readonly IColorRepository _colorRepository;
        public CreateColorHandler(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        public async Task<CommandResult> Handle(CreateColorRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _colorRepository.GetSingleAsync(c => c.Name == request.Name);
            if(check != null)
            {
                result.Success = false;
                result.Message = "Color found.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var color = new Color
            {
                Name = request.Name,
            };

            await _colorRepository.Add(color);
            return result;
        }
    }
}
