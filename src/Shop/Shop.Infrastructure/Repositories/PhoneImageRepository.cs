using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class PhoneImageRepository : GenericRepository<PhoneImage, int>, IPhoneImageRepository
    {
        private readonly AppDbContext _context;
        public PhoneImageRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
