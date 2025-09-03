using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PhoneVariants;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PhoneVariants
{
    public class UpdatePhoneVariantHandler : IRequestHandler<UpdatePhoneVariantRequest, CommandResult>
    {
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IRamRepository _ramRepository;
        private readonly IStorageRepository _storageRepository;
        public UpdatePhoneVariantHandler(IPhoneVariantRepository phoneVariantRepository, IPhoneRepository phoneRepository, IColorRepository colorRepository, IRamRepository ramRepository, IStorageRepository storageRepository)
        {
            _phoneVariantRepository = phoneVariantRepository;
            _phoneRepository = phoneRepository;
            _colorRepository = colorRepository;
            _ramRepository = ramRepository;
            _storageRepository = storageRepository;
        }
        public async Task<CommandResult> Handle(UpdatePhoneVariantRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var variant = await _phoneVariantRepository.GetByIdAsync(request.VariantId);
            if (variant is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneVariant));
                result.Code = StatusCode.NotFound;
                return result;
            }

            if (request.PhoneId != null)
            {
                var phoneExists = await _phoneRepository.Exists((int)request.PhoneId);
                if (!phoneExists)
                {
                    result.Success = false;
                    result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                    result.Code = StatusCode.NotFound;
                    return result;
                }
            }

            if (request.ColorId != null)
            {
                var colorExists = await _colorRepository.Exists((int)request.ColorId);
                if (!colorExists)
                {
                    result.Success = false;
                    result.Message = string.Format(CommonMessages.NotFound, nameof(Color));
                    result.Code = StatusCode.NotFound;
                    return result;
                }
            }

            if (request.RamId != null)
            {
                var ramExists = await _ramRepository.Exists((int)request.RamId);
                if (!ramExists)
                {
                    result.Success = false;
                    result.Message = string.Format(CommonMessages.NotFound, nameof(Ram));
                    result.Code = StatusCode.NotFound;
                    return result;
                }
            }

            if (request.StorageId != null)
            {
                var storageExists = await _storageRepository.Exists((int)request.StorageId);
                if (!storageExists)
                {
                    result.Success = false;
                    result.Message = string.Format(CommonMessages.NotFound, nameof(Storage));
                    result.Code = StatusCode.NotFound;
                    return result;
                }
            }


            var updateEntity = new PhoneVariant
            {
                PhoneId = request.PhoneId ?? variant.PhoneId,
                ColorId = request.ColorId ?? variant.ColorId,
                RamId = request.RamId ?? variant.RamId,
                StorageId = request.StorageId ?? variant.StorageId,
                Price = request.Price ?? variant.Price,
                StockQuantity = request.StockQuantity ?? variant.StockQuantity,
            };
            variant.UpdateWith(updateEntity);
            await _phoneVariantRepository.Update(variant);

            return result;
        }
    }
}
