using System.Security.Cryptography;
using System.Text;

namespace InnoShop.Users.API.Models
{
    /// <summary>
    /// User login info.
    /// </summary>
    public class UserLoginInfo
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public string PasswordHash => CalcMD5Hash(Password);

        public static string CalcMD5Hash(string message)
        {
            var hashBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(message));
            var hexString = string.Concat(hashBytes.Select(b => b.ToString("x2")));
            return hexString;
        }

        public static string GenerateRandomPassword(int length = 8)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Range(0, length)
                                        .Select(_ => chars[random.Next(chars.Length)])
                                        .ToArray());
            return result;
        }
    }
}
