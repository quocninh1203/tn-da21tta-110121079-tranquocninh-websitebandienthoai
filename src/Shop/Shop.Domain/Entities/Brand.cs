namespace Shop.Domain.Entities
{
    public class Brand : BaseEntities<int>
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? ImageUrl { get; set; }
    }
}
