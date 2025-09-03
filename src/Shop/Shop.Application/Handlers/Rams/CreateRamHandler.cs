using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Rams;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Rams
{
    public class CreateRamHandler : IRequestHandler<CreateRamRequest, CommandResult>
    {
        private readonly IRamRepository _ramRepository;
        public CreateRamHandler(IRamRepository ramRepository)
        {
            _ramRepository = ramRepository;
        }
        public async Task<CommandResult> Handle(CreateRamRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _ramRepository.GetSingleAsync(r => r.Size == request.Size);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Ram));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var ram = new Ram
            {
                Size = request.Size,
            };
            await _ramRepository.Add(ram);

            return result;
        }
    }
}
