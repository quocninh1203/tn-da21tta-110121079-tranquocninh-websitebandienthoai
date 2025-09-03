namespace Shop.Domain.Entities
{
    public class Review : BaseEntities<int>
    {
        public int UserId { get; set; }
        public int PhoneId { get; set; }
        public int VariantId { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public DateTime CreateDate { get; set; }
        public string? ImageUrl { get; set; } 
    }
}
