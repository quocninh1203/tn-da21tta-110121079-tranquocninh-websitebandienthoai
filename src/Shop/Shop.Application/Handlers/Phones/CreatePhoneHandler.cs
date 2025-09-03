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
    public class CreatePhoneHandler : IRequestHandler<CreatePhoneRequest, CommandResult>
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUploadService _fileUploadService;
        public CreatePhoneHandler(IPhoneRepository phoneRepository, IBrandRepository brandRepository, IFileUploadService fileUploadService )
        {
            _phoneRepository = phoneRepository;
            _brandRepository = brandRepository;
            _fileUploadService = fileUploadService;
        }
        public async Task<CommandResult> Handle(CreatePhoneRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Brand));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var phone = new Phone
            {
                Name = request.Name,
                BrandId = request.BrandId,
                Screen = request.Screen,
                Chip = request.Chip,
                Battery = request.Battery,
                Description = request.Description,
                Slug = StringExtensions.ToSlug(request.Name),
                IsActive = request.IsActive,
            };

            await _phoneRepository.Add(phone);

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                phone.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(Phone), phone.Id);
                await _phoneRepository.Update(phone);
            }

            return result;
        }
    }
}
