using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Colors;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Colors
{
    public class UpdateColorHandler : IRequestHandler<UpdateColorRequest, CommandResult>
    {
        private readonly IColorRepository _colorRepository;
        public UpdateColorHandler(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        public async Task<CommandResult> Handle(UpdateColorRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var color = await _colorRepository.GetByIdAsync(request.ColorId);
            if (color is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Ram));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new Color
            {
                Name = request.Name,
            };
            color.UpdateWith(updateEntity);

            var check = await _colorRepository.GetSingleAsync(r => r.Name == color.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Color));
                result.Code = StatusCode.Conflict;
                return result;
            }

            await _colorRepository.Update(color);
            return result;
        }
    }
}
