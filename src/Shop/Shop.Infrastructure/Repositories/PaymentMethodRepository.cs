using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Sql;

namespace Shop.Infrastructure.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod, int>, IPaymentMethodRepository
    {
        private readonly AppDbContext _context;
        public PaymentMethodRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }
    }
}
