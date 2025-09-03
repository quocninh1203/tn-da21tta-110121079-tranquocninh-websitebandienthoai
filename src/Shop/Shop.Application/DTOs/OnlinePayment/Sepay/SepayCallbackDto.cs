namespace Shop.Application.DTOs.OnlinePayment.Sepay
{
    public class SepayCallbackDto
    {
        public string OrderCode { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // "success", "failed"
        public string TransactionId { get; set; }
    }
}
