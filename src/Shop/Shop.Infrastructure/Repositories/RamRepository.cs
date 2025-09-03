using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class RamRepository : GenericRepository<Ram, int>, IRamRepository
    {
        private readonly AppDbContext _context;
        public RamRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
