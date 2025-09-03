using Shop.Application.DTOs.OnlinePayment.Sepay;

namespace Shop.Application.Services.Sepay
{
    public interface ISePayService
    {
        Task<string> CreatePaymentAsync(SepayPaymentRequest request);
        Task HandleCallbackAsync(SepayCallbackDto data);
    }

}
