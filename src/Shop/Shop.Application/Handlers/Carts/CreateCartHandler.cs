using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Carts;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Carts
{
    public class CreateCartHandler : IRequestHandler<CreateCartRequest, CommandResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhoneVariantRepository _userVariantRepository;
        public CreateCartHandler(ICartRepository cartRepository, IUserRepository userRepository, IPhoneVariantRepository userVariantRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _userVariantRepository = userVariantRepository;
        }
        public async Task<CommandResult> Handle(CreateCartRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(User));
                result.Code = StatusCode.NotFound;
                return result;
            }
            var variant = await _userVariantRepository.GetByIdAsync(request.VariantId);
            if (variant == null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(PhoneVariant));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var cart = new Cart
            {
                UserId = request.UserId,
                VariantId = request.VariantId,
                Quantity = request.Quantity,
                AddedDate = request.AddedDate,
            };

            await _cartRepository.Add(cart);

            result.Success = true;
            result.Message = "Thêm vào giỏ hàng thành công.";
            result.Code = StatusCode.Ok;

            return result;
        }
    }
}
