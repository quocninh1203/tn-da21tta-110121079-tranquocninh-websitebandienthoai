using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class PhoneRepository : GenericRepository<Phone, int>, IPhoneRepository
    {
        private readonly AppDbContext _context;
        public PhoneRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
