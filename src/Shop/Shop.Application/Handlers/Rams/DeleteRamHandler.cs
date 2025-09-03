using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Rams;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Rams
{
    public class DeleteRamHandler : IRequestHandler<DeleteRamRequest, CommandResult>
    {
        private readonly IRamRepository _ramRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        public DeleteRamHandler(IRamRepository ramRepository, IPhoneVariantRepository phoneVariantRepository)
        {
            _ramRepository = ramRepository;
            _phoneVariantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(DeleteRamRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var variant = await _phoneVariantRepository.GetAsync(p => p.RamId == request.RamId);
            if (variant.Count > 0)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.CannotDelete, nameof(Ram));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var ram = await _ramRepository.GetByIdAsync(request.RamId);
            if (ram is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Ram));
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _ramRepository.Delete(ram);

            return result;
        }
    }
}
