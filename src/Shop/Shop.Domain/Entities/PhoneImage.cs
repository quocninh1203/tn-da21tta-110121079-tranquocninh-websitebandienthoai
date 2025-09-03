namespace Shop.Domain.Entities
{
    public class PhoneImage : BaseEntities<int>
    {
        public int PhoneId { get; set; }
        public string ImageUrl { get; set; }
        public Phone Phone { get; set; }
    }
}
