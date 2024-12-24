namespace InnoShop.Products.API.Models
{
    public class ProductDto
    {
        public required string Title { get; set; }

        public required string Description { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}
