using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
