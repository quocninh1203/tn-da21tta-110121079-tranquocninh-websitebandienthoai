using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class ShipperRepository : GenericRepository<Shipper, int>, IShipperRepository
    {
        private readonly AppDbContext _context;
        public ShipperRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
