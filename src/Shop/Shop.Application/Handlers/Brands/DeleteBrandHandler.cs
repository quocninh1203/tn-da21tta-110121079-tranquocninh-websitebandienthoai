using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Brands;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Brands
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandRequest, CommandResult>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IPhoneRepository _phoneRepository;
        public DeleteBrandHandler(IBrandRepository brandRepository, IPhoneRepository phoneRepository)
        {
            _brandRepository = brandRepository;
            _phoneRepository = phoneRepository;
        }
        public async Task<CommandResult> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var phones = await _phoneRepository.GetAsync(p => p.BrandId ==  request.BrandId);
            if(phones.Count > 0)
            {
                result.Success = false;
                result.Message = "Brand đã được sử dụng.";
                result.Code = StatusCode.Conflict;
                return result;
            }

            var brand = await _brandRepository.GetByIdAsync(request.BrandId);
            if (brand is null)
            {
                result.Success = false;
                result.Message = "Brand không tồn tại.";
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _brandRepository.Delete(brand);

            return result;
        }
    }
}
