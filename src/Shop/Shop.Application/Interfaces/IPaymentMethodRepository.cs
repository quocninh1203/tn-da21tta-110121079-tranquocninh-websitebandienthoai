using Shop.Domain.Entities;

namespace Shop.Application.Interfaces
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod, int>
    {
    }
}
