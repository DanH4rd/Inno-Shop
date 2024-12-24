using InnoShop.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Users.API.Infrastructure
{
    /// <summary>
    /// Class describes InnoShop Users data context.
    /// </summary>
    public class UsersDbContext : DbContext
    {
        public required DbSet<User> Users { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set Id as primary key
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            // Email should be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");

            // other indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.PasswordHash)
                .HasDatabaseName("IX_User_PasswordHash");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .HasDatabaseName("IX_User_Name");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.CreatedDate)
                .HasDatabaseName("IX_User_CreatedDate");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.LastLogin)
                .HasDatabaseName("IX_User_LastLogin");
        }
    }
}
