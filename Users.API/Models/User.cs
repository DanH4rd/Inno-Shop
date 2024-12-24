using System.ComponentModel.DataAnnotations;

namespace InnoShop.Users.API.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public required string Email { get; set; }

        [MaxLength(255)]
        public required string EmailConfirmationToken { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [MaxLength(255)]
        public required string PasswordHash { get; set; }

        public UserRoles Role { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; } = default(DateTime?);

        public bool IsActive { get; set; }
    }

    public enum UserRoles
    {
        Admin = 1,
        User = 2
    }
}
