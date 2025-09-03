using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.Carts;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.Carts
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartRequest, CommandResult>
    {
        private readonly ICartRepository _cartRepository;
        public DeleteCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CommandResult> Handle(DeleteCartRequest request, CancellationToken cancellationToken)
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

            await _cartRepository.Delete(cart);
            return result;
        }
    }
}
