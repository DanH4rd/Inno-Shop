using InnoShop.Users.API.Abstract;

namespace InnoShop.Users.API.Infrastructure
{
    /// <summary>
    /// Email provider.
    /// For demonstration purposes only, doesn't really send emails.
    /// </summary>
    /// <remarks>
    /// This implementation is a mock and should not be used in production.
    /// </remarks>
    public class MockEmailProvider : IEmailProvider
    {
        private readonly ILogger<MockEmailProvider> _logger;

        public MockEmailProvider(ILogger<MockEmailProvider> logger) => _logger = logger;

        public async Task SendAsync(string email, string subject, string body)
        {
            // TODO: implement send logic

            _logger.LogInformation("Email is sent to '{0}' with subject '{1}' and body '{2}'.", email, subject, body);

            await Task.CompletedTask;
        }
    }
}
