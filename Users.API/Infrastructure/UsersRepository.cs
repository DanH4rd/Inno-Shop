using InnoShop.Users.API.Abstract;
using InnoShop.Users.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Users.API.Infrastructure
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByLoginInfoAsync(UserLoginInfo loginInfo)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginInfo.Email
                                     && u.PasswordHash == loginInfo.PasswordHash
                                     && u.IsActive);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                _context.SaveChanges();
            }
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash,
                Role = userDto.Role,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow,
                IsEmailConfirmed = false,
                IsActive = true,
                LastLogin = null
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            return await SetUserAvailability(id, false);
        }

        public async Task<int> RestoreUserAsync(int id)
        {
            return await SetUserAvailability(id, true);
        }

        private async Task<int> SetUserAvailability(int id, bool availability)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return 0;

            user.IsActive = availability;
            return await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> UpdateUserAsync(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(userDto.Name)) user.Name = userDto.Name;
            // update email if it's a new one and reset email confirmation
            if (!string.IsNullOrWhiteSpace(userDto.Email)
                && !userDto.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                user.Email = userDto.Email;
                user.EmailConfirmationToken = Guid.NewGuid().ToString();
                user.IsEmailConfirmed = false;
            }
            if (!string.IsNullOrEmpty(userDto.Password)) user.PasswordHash = userDto.PasswordHash;
            user.Role = userDto.Role;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<int> ConfirmUserEmailAsync(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailConfirmationToken == token);
            if (user == null) return 0;

            user.IsEmailConfirmed = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<string> RequestPasswordRestoreAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return string.Empty;

            // use EmailConfirmationToken for password request validation for simplicity
            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();
            return user.EmailConfirmationToken;
        }

        public async Task<(string, string)> RestorePasswordAsync(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.EmailConfirmationToken == token);
            if (user == null) return (string.Empty, string.Empty);

            var passw = UserLoginInfo.GenerateRandomPassword();
            user.PasswordHash = UserLoginInfo.CalcMD5Hash(passw);
            await _context.SaveChangesAsync();
            return (user.Email, passw);
        }
    }
}
