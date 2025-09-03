using Shop.Application.DTOs.Orders;

namespace Shop.Application.Services.Email
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string otpCode);
        Task SendOrderConfirmationAsync(
            string toEmail,
            string customerName,
            string phoneNumber,
            string address,
            string orderCode,
            DateTime orderDate,
            int totalPrice,
            List<OrderItemDTO> item,
            string paymentMethod,
            int shippingFee,
            bool isPaid
        );
    }
}
