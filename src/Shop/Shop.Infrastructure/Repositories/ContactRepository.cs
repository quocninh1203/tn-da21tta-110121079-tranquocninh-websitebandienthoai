using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class ContactRepository : GenericRepository<Contact, int>, IContactRepository
    {
        private readonly AppDbContext _context;
        public ContactRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
