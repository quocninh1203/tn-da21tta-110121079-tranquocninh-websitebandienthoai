using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart, int>
    {
    }
}
