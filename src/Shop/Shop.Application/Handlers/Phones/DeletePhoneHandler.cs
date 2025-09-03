using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class DeletePhoneHandler : IRequestHandler<DeletePhoneRequest, CommandResult>
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IPhoneVariantRepository _variantRepository;
        public DeletePhoneHandler(IPhoneRepository phoneRepository, IPhoneVariantRepository phoneVariantRepository)
        {
            _phoneRepository = phoneRepository;
            _variantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(DeletePhoneRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();
            var variants = await _variantRepository.GetAsync(p => p.PhoneId == request.PhoneId);
            if (variants.Count > 0)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.CannotDelete, nameof(Phone));
                result.Code = StatusCode.Conflict;
                return result;
            }

            var phone = await _phoneRepository.GetByIdAsync(request.PhoneId);
            if (phone is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Phone));
                result.Code = StatusCode.NotFound;
                return result;
            }
            await _phoneRepository.Delete(phone);

            return result;
        }
    }
}
