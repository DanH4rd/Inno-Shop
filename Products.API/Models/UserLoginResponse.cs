namespace InnoShop.Products.API.Models
{
    /// <summary>
    /// Describes successful auth response.
    /// </summary>
    public class UserLoginResponse
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
