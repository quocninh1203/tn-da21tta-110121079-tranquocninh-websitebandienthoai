using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail, int>
    {
    }
}
