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
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandRequest, CommandResult>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFileUploadService _fileUploadService;

        public UpdateBrandHandler(IBrandRepository brandRepository, IFileUploadService fileUploadService)
        {
            _brandRepository = brandRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<CommandResult> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand is null)
            {
                result.Success = false;
                result.Message = "Brand không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }

            // Cập nhật tên và slug
            var updateEntity = new Brand
            {
                Name = request.Name,
                Slug = StringExtensions.ToSlug(request.Name),
            };
            brand.UpdateWith(updateEntity);

            // Nếu có ảnh mới được gửi
            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                // Xoá ảnh cũ nếu có
                if (!string.IsNullOrWhiteSpace(brand.ImageUrl))
                {
                    _fileUploadService.RemoveImage(brand.ImageUrl);
                }

                // Lưu ảnh mới
                brand.ImageUrl = _fileUploadService.SaveImage(request.ImageBase64, nameof(Brand), brand.Id);
            }

            await _brandRepository.Update(brand);
            return result;
        }
    }
}
