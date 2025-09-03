using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Carts;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Carts
{
    public class UpdateQuantityHandler : IRequestHandler<UpdateCartRequest, CommandResult>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPhoneVariantRepository _phoneVariantRepository;
        public UpdateQuantityHandler(ICartRepository cartRepository, IPhoneVariantRepository phoneVariantRepository)
        {
            _cartRepository = cartRepository;
            _phoneVariantRepository = phoneVariantRepository;
        }
        public async Task<CommandResult> Handle(UpdateCartRequest request, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var cart = await _cartRepository.GetByIdAsync(request.CartId);
            if (cart is null)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.NotFound, nameof(Cart));
                result.Code = StatusCode.NotFound;
                return result;
            }

            var variant = await _phoneVariantRepository.GetByIdAsync(cart.VariantId);
            if (request.Quantity > variant.StockQuantity)
            {
                result.Success = false;
                result.Message = string.Format(CommonMessages.ExceedsStock);
                result.Code = StatusCode.BadRequest;
                return result;
            }

            cart.Quantity = (int)request.Quantity;

            await _cartRepository.Update(cart);
            return result;
        }
    }
}
