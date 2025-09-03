using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<User, int>, IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task DeleteRefreshTokenAsync(int userId)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetRefreshTokenAsync(int userId)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            return user?.RefreshToken;
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                await _context.SaveChangesAsync();
            }
        }
    }
}
