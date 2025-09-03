using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, int>
    {
    }
}
