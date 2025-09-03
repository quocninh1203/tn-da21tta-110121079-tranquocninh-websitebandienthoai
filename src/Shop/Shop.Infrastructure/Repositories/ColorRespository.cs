using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class ColorRespository : GenericRepository<Color, int>, IColorRepository
    {
        private readonly AppDbContext _context;
        public ColorRespository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
