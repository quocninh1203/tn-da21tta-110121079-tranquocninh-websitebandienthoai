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
    public class CreatePhoneImageHandler : IRequestHandler<CreatePhoneImageRequest, CommandResult>
    {
        private readonly IPhoneImageRepository _phoneImageRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IFileUploadService _fileUploadService;
        public CreatePhoneImageHandler(IPhoneImageRepository phoneImageRepository, IPhoneRepository phoneRepository, IFileUploadService fileUploadService)
        {
            _phoneImageRepository = phoneImageRepository;
            _phoneRepository = phoneRepository;
            _fileUploadService = fileUploadService;
        }
        public async Task<CommandResult> Handle(CreatePhoneImageRequest request, CancellationToken cancellationToken)
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

            var phoneImage = new PhoneImage
            {
                PhoneId = request.PhoneId
            };

            await _phoneImageRepository.Add(phoneImage);

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                phoneImage.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(PhoneImage), phoneImage.Id);
                await _phoneImageRepository.Update(phoneImage);
            }

            return result;
        }
    }
}
