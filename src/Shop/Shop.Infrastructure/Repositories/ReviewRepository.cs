using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review, int>, IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
