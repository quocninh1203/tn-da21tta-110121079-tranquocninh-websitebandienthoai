using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Colors;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Colors
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorRequest, CommandResult>
    {
        private readonly IColorRepository _colorRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        public DeleteColorHandler(IColorRepository colorRepository, IPhoneVariantRepository phoneVariantRepository)
        {
            _colorRepository = colorRepository;
            _phoneVariantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(DeleteColorRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var variant = await _phoneVariantRepository.GetAsync(p => p.RamId == request.ColorId);
            if (variant.Count > 0)
            {
                result.Success = false;
                result.Message = "Color đã được sử dụng.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var color = await _colorRepository.GetByIdAsync(request.ColorId);
            if (color is null)
            {
                result.Success = false;
                result.Message = "Color không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _colorRepository.Delete(color);

            return result;
        }
    }
}
