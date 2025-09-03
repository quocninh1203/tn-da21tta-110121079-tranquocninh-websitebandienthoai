using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class StorageRepository : GenericRepository<Storage, int>, IStorageRepository
    {
        private readonly AppDbContext _context;
        public StorageRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
