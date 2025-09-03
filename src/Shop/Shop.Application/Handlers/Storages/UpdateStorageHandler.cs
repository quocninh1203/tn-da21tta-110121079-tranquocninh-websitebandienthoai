using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Storages;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Storages
{
    public class UpdateStorageHandler : IRequestHandler<UpdateStorageRequest, CommandResult>
    {
        private readonly IStorageRepository _storageRepository;
        public UpdateStorageHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        public async Task<CommandResult> Handle(UpdateStorageRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var storage = await _storageRepository.GetByIdAsync(request.StorageId);
            if (storage is null)
            {
                result.Success = false;
                result.Message = "Storage không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new Storage
            {
                Size = request.Size,
            };
            storage.UpdateWith(updateEntity);

            var check = await _storageRepository.GetSingleAsync(r => r.Size == storage.Size);
            if (check != null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.AlreadyExists, nameof(Storage));
                result.Code = StatusCode.Conflict;
                return result;
            }

            await _storageRepository.Update(storage);
            return result;
        }
    }
}
