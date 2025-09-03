using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PhoneVariants;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PhoneVariants
{
    public class CreatePhoneVariantHandler : IRequestHandler<CreatePhoneVariantRequest, CommandResult>
    {
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IRamRepository _ramRepository;
        private readonly IStorageRepository _storageRepository;
        public CreatePhoneVariantHandler(IPhoneVariantRepository phoneVariantRepository, IPhoneRepository phoneRepository, IColorRepository colorRepository, IRamRepository ramRepository, IStorageRepository storageRepository)
        {
            _phoneVariantRepository = phoneVariantRepository;
            _phoneRepository = phoneRepository;
            _colorRepository = colorRepository;
            _ramRepository = ramRepository;
            _storageRepository = storageRepository;
        }
        public async Task<CommandResult> Handle(CreatePhoneVariantRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var phone = await _phoneRepository.GetByIdAsync(request.PhoneId);
            if (phone == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var color = await _colorRepository.GetByIdAsync(request.ColorId);
            if (color == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Color));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var ram = await _ramRepository.GetByIdAsync(request.RamId);
            if (ram == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Ram));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var storage = await _storageRepository.GetByIdAsync(request.StorageId);
            if (storage == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Storage));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var variant = new PhoneVariant
            {
                PhoneId = request.PhoneId,
                ColorId = request.ColorId,
                RamId = request.RamId,
                StorageId = request.StorageId,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };

            await _phoneVariantRepository.Add(variant);
            return result;
        }
    }
}
