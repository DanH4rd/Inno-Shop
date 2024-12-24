using InnoShop.Users.API.Infrastructure;
using MediatR;

namespace InnoShop.Users.API.Abstract
{
    public abstract class NotificationHandlerBase
    {
        protected readonly IEmailProvider? _emailProvider;
        protected readonly IProductsProvider? _productsProvider;
        protected readonly ILogger<NotificationHandlerBase>? _logger;
        public NotificationHandlerBase(IEmailProvider? emailProvider, IProductsProvider? productsProvider, ILogger<NotificationHandlerBase>? logger)
        {
            _emailProvider = emailProvider;
            _productsProvider = productsProvider;
            _logger = logger;
        }
    }

    public class UserModifiedNotificationHandler : NotificationHandlerBase, INotificationHandler<UserModifiedNotification>
    {
        public UserModifiedNotificationHandler(IEmailProvider emailProvider) : base(emailProvider, null, null) { }

        public async Task Handle(UserModifiedNotification notification, CancellationToken cancellationToken)
        {
            await _emailProvider!.SendAsync(notification.user.Email,
                "InnoShop: email confirmation",
                string.Format("users/emailconfirm/{0}", notification.user.EmailConfirmationToken));
        }
    }

    public class UserInfoRequestedNotificationHandler : NotificationHandlerBase, INotificationHandler<UserInfoRequestedNotification>
    {
        public UserInfoRequestedNotificationHandler(IEmailProvider emailProvider) : base(emailProvider, null, null) { }

        public async Task Handle(UserInfoRequestedNotification notification, CancellationToken cancellationToken)
        {
            await _emailProvider!.SendAsync(notification.email, notification.subject, notification.content);
        }
    }

    public class UserActiveStatusChangedNotificationHandler : NotificationHandlerBase, INotificationHandler<UserActiveStatusChangedNotification>
    {
        public UserActiveStatusChangedNotificationHandler(IProductsProvider productsProvider, ILogger<NotificationHandlerBase> logger)
            : base(null, productsProvider, logger) { }

        public async Task Handle(UserActiveStatusChangedNotification notification, CancellationToken cancellationToken)
        {
            // simple retries logic in case of failed request to products.api
            // it can be improved by using messages queue like RabbitMQ to save failed operations and periodically retry

            const int maxRetries = 3;
            int attempt = 0;
            bool success = false;
            string? error = null;

            do
            {
                attempt++;
                (success, error) = await _productsProvider!.AdjustUserAvailabilityAsync(notification.userId, notification.isActive);

                if (!success)
                {
                    _logger?.LogWarning($"Attempt {attempt} for user Id {notification.userId}, availability to set {notification.isActive} failed: {error}");
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2)); // await before next try
                    }
                }
            }
            while (!success && attempt < maxRetries);

            if (!success)
            {
                // here we can save failed operation to a queue
                _logger?.LogWarning($"All retry attempts failed for user Id: {notification.userId}, availability to set: {notification.isActive}.");
            }
        }
    }

}
