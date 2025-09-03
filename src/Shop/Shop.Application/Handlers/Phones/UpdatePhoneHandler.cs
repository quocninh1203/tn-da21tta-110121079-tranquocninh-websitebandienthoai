using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Application.Services.FileUpload;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class UpdatePhoneHandler : IRequestHandler<UpdatePhoneRequest, CommandResult>
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUploadService _fileUploadService;

        public UpdatePhoneHandler(
            IBrandRepository brandRepository,
            IPhoneRepository phoneRepository,
            IFileUploadService fileUploadService)
        {
            _phoneRepository = phoneRepository;
            _brandRepository = brandRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<CommandResult> Handle(UpdatePhoneRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var phone = await _phoneRepository.GetByIdAsync(request.PhoneId);
            if (phone is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                return result;
            }

            if(request.BrandId != null)
            {
                var brandExists = await _brandRepository.Exists((int)request.BrandId);
                if (!brandExists)
                {
                    result.Success = false;
                    result.Message = string.Format(CommonMessages.NotFound, nameof(Brand));
                    result.Code = StatusCode.NotFound;
                    return result;
                }
            }

            var updateEntity = new Phone
            {
                Name = request.Name,
                BrandId = request.BrandId ?? phone.BrandId,
                ImageUrl = request.ImageBase64,
                Screen = request.Screen,
                Chip = request.Chip,
                Battery = request.Battery,
                Description = request.Description,
                Slug = StringExtensions.ToSlug(request.Name),
                IsActive = phone.IsActive
            };
            phone.UpdateWith(updateEntity);

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                // Xoá ảnh cũ nếu có
                if (!string.IsNullOrWhiteSpace(phone.ImageUrl))
                {
                    _fileUploadService.RemoveImage(phone.ImageUrl);
                }

                // Lưu ảnh mới
                phone.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(Phone), phone.Id);
            }

            await _phoneRepository.Update(phone);
            return result;
        }
    }
}
