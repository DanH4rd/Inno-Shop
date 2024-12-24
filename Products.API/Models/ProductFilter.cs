namespace InnoShop.Products.API.Models
{
    /// <summary>
    /// Represents parameters to filter products list.
    /// </summary>
    public class ProductFilter
    {
        public string? SearchText { get; set; }

        public bool? IsAvailable { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public DateTime? CreatedAfter { get; set; }

        public DateTime? CreatedBefore { get; set; }
    }
}
