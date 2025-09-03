using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Rams;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Rams
{
    public class UpdateRamHandler : IRequestHandler<UpdateRamRequest, CommandResult>
    {
        private readonly IRamRepository _ramRepository;
        public UpdateRamHandler(IRamRepository ramRepository)
        {
            _ramRepository = ramRepository;
        }
        public async Task<CommandResult> Handle(UpdateRamRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var ram = await _ramRepository.GetByIdAsync(request.RamId);
            if (ram is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Ram));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new Ram
            {
                Size = request.Size,
            };
            ram.UpdateWith(updateEntity);

            var check = await _ramRepository.GetSingleAsync(r => r.Size == ram.Size);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Ram));
                result.Code = StatusCode.Conflict;
                return result;
            }

            await _ramRepository.Update(ram);
            return result;
        }
    }
}
