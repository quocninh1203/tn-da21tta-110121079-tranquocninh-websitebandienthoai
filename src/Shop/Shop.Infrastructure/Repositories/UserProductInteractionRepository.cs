using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class UserProductInteractionRepository : GenericRepository<UserProductInteraction, int>, IUserProductInteractionRepository
    {
        private readonly AppDbContext _context;
        public UserProductInteractionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
