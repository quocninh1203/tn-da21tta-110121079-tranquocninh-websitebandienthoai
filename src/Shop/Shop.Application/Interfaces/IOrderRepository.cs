using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order, int>
    {
    }
}
