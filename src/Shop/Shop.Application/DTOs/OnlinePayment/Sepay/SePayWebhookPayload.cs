namespace Shop.Application.DTOs.OnlinePayment.Sepay
{
    public class SePayWebhookPayload
    {
        public int Id { get; set; }
        public string Gateway { get; set; }
        public string TransactionDate { get; set; }  // Giữ nguyên kiểu string để tránh lỗi parse
        public string AccountNumber { get; set; }
        public string SubAccount { get; set; }
        public string? Code { get; set; }
        public string Content { get; set; }
        public string TransferType { get; set; }
        public string Description { get; set; }
        public decimal TransferAmount { get; set; }
        public string ReferenceCode { get; set; }
        public decimal Accumulated { get; set; }
    }
}
