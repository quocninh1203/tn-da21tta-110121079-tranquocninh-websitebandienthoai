using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Phones;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Phones
{
    public class ActivatePhoneHandler : IRequestHandler<ActivatePhoneRequest, CommandResult>
    {
        private readonly IPhoneRepository _phoneRepository;
        public ActivatePhoneHandler(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }
        public async Task<CommandResult> Handle(ActivatePhoneRequest request, CancellationToken cancellationToken)
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
            phone.IsActive = !phone.IsActive;

            await _phoneRepository.Update(phone);
            return result;
        }
    }
}
