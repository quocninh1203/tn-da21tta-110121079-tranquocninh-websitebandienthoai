using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PhoneImages;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PhoneImages
{
    public class DeletePhoneImageHandler : IRequestHandler<DeletePhoneImageRequest, CommandResult>
    {
        private readonly IPhoneImageRepository _repository;
        public DeletePhoneImageHandler(IPhoneImageRepository phoneImageRepository)
        {
            _repository = phoneImageRepository;
        }
        public async Task<CommandResult> Handle(DeletePhoneImageRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var img = await _repository.GetByIdAsync(request.ImgId);
            if (img is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneImage));
                result.Code = StatusCode.NotFound;
                return result;
            }

            await _repository.Delete(img);
            return result;
        }
    }
}
