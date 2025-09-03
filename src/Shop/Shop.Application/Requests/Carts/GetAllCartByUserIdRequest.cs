using MediatR;
using Shop.Application.DTOs.Cart;
using Shop.Domain.Results;

namespace Shop.Application.Requests.Carts
{
    public class GetAllCartByUserIdRequest : IRequest<QueryResult<List<CartDTO>>>
    {
        public int UserId { get; set; }
    }
}
