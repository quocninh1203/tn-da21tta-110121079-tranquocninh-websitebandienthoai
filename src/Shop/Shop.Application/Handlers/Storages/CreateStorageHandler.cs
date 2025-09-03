using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Storages;
using Shop.Domain.Entities;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Storages
{
    public class CreateStorageHandler : IRequestHandler<CreateStorageRequest, CommandResult>
    {
        private readonly IStorageRepository _storageRepository;
        public CreateStorageHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        public async Task<CommandResult> Handle(CreateStorageRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _storageRepository.GetSingleAsync(st => st.Size == request.Size);
            if (check != null)
            {
                result.Success = false;
                result.Message = "Storage found.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var storage = new Storage
            {
                Size = request.Size,
            };
            await _storageRepository.Add(storage);

            return result;
        }
    }
}
