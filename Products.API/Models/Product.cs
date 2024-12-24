using System.ComponentModel.DataAnnotations;

namespace InnoShop.Products.API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(255)]
        public required string Title { get; set; }

        [MaxLength(3000)]
        public required string Description { get; set; }

        public double Price { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsUserActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; } = default(DateTime?);

    }
}
