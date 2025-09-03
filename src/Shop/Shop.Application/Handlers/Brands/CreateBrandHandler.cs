using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Brands;
using Shop.Application.Services.FileUpload;
using Shop.Domain.Entities;
using Shop.Domain.Methods;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Brands
{
    public class CreateBrandHandler : IRequestHandler<CreateBrandRequest, CommandResult>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUploadService _fileUploadService;
        public CreateBrandHandler(IBrandRepository brandRepository, IFileUploadService fileUploadService)
        {
            _brandRepository = brandRepository;
            _fileUploadService = fileUploadService;
        }
        public async Task<CommandResult> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var check = await _brandRepository.GetSingleAsync(b => b.Name == request.Name);
            if (check != null)
            {
                result.Success = false;
                result.Message = "Brand found.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var brand = new Brand
            {
                Name = request.Name,
                Slug = StringExtensions.ToSlug(request.Name),
            };
            await _brandRepository.Add(brand);

            brand.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(Brand).ToString(), brand.Id);
            await _brandRepository.Update(brand);

            return result;
        }
    }
}
