using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class PhoneVariantRepository : GenericRepository<PhoneVariant, int>, IPhoneVariantRepository
    {
        private readonly AppDbContext _context;
        public PhoneVariantRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
