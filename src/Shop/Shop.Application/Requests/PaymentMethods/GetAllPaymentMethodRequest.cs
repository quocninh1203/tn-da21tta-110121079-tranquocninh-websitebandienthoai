using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Results;

namespace Shop.Application.Requests.PaymentMethods
{
    public class GetAllPaymentMethodRequest : IRequest<QueryResult<List<PaymentMethod>>>
    {
    }
}
