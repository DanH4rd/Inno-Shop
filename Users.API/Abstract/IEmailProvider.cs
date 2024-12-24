namespace InnoShop.Users.API.Abstract
{
    /// <summary>
    /// Interface describing email provider.
    /// </summary>
    public interface IEmailProvider
    {
        Task SendAsync(string email, string subject, string body);
    }
}
