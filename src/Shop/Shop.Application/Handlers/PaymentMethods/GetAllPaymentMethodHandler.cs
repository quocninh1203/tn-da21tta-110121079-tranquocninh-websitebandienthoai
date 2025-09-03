using MediatR;
using Shop.Application.Interfaces;
using Shop.Application.Requests.PaymentMethods;
using Shop.Domain.Entities;
using Shop.Domain.Messages;
using Shop.Domain.Results;
using Shop.Domain.StatusCodes;

namespace Shop.Application.Handlers.PaymentMethods
{
    public class GetAllPaymentMethodHandler : IRequestHandler<GetAllPaymentMethodRequest, QueryResult<List<PaymentMethod>>>
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        public GetAllPaymentMethodHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        public async Task<QueryResult<List<PaymentMethod>>> Handle(GetAllPaymentMethodRequest request, CancellationToken cancellationToken)
        {
            var payMethods = await _paymentMethodRepository.GetAsync();
            var response = new QueryResult<List<PaymentMethod>>();

            if (payMethods == null || !payMethods.Any())
            {
                response.Success = false;
                response.Message = string.Format(CommonMessages.NoDataFound, nameof(PaymentMethod));
                response.Code = StatusCode.NotFound;
                response.Model = new List<PaymentMethod>();
            }
            response.Model = (List<PaymentMethod>)payMethods;

            return response;
        }
    }
}
