using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class BrandRepository : GenericRepository<Brand, int>, IBrandRepository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
