using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PhoneImages;
using Shop.Application.Services.FileUpload;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PhoneImages
{
    public class UpdatePhoneImageHandler : IRequestHandler<UpdatePhoneImageRequest, CommandResult>
    {
        private readonly IPhoneImageRepository _phoneImageRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IFileUploadService _fileUploadService;
        public UpdatePhoneImageHandler(IPhoneImageRepository phoneImageRepository, IPhoneRepository phoneRepository, IFileUploadService fileUploadService)
        {
            _phoneImageRepository = phoneImageRepository;
            _phoneRepository = phoneRepository;
            _fileUploadService = fileUploadService;
        }
        public async Task<CommandResult> Handle(UpdatePhoneImageRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var img = await _phoneImageRepository.GetByIdAsync(request.ImgId);
            if (img is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneImage));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var phoneExists = await _phoneRepository.Exists((int)request.PhoneId);
            if (!phoneExists)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var updateEntity = new PhoneImage
            {
                PhoneId = (int)request.PhoneId,
            };
            img.UpdateWith(updateEntity);

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                // Xoá ảnh cũ nếu có
                if (!string.IsNullOrWhiteSpace(img.ImageUrl))
                {
                    _fileUploadService.RemoveImage(img.ImageUrl);
                }

                // Lưu ảnh mới
                img.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(PhoneImage), img.Id);
            }

            await _phoneImageRepository.Update(img);
            return result;
        }
    }
}
