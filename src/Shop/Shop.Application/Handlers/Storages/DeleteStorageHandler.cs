using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Storages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Storages
{
    public class DeleteStorageHandler : IRequestHandler<DeleteStorageRequest, CommandResult>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        public DeleteStorageHandler(IStorageRepository storageRepository, IPhoneVariantRepository phoneVariantRepository)
        {
            _storageRepository = storageRepository;
            _phoneVariantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(DeleteStorageRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var variant = await _phoneVariantRepository.GetAsync(p => p.StorageId == request.StorageId);
            if (variant.Count > 0)
            {
                result.Success = false;
                result.Message = "Storage đã được sử dụng.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var storage = await _storageRepository.GetByIdAsync(request.StorageId);
            if (storage is null)
            {
                result.Success = false;
                result.Message = "Ram không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _storageRepository.Delete(storage);

            return result;
        }
    }
}
