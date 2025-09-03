using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class OtpRepository : GenericRepository<OTP, int>, IOtpRepository
    {
        private readonly AppDbContext _context;
        public OtpRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
