namespace Shop.Application.DTOs.OnlinePayment.Sepay
{
    public class SepayPaymentRequest
    {
        public string OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
    }
}
