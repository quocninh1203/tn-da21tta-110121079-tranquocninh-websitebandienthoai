using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<User, int>
    {
        Task SaveRefreshTokenAsync(int userId, string refreshToken);
        Task<string> GetRefreshTokenAsync(int userId);
        Task DeleteRefreshTokenAsync(int userId);
    }
}
