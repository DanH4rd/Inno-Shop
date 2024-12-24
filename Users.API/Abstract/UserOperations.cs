using MediatR;
using InnoShop.Users.API.Models;

namespace InnoShop.Users.API.Abstract
{
    #region Queries

    public record GetUsersQuery() : IRequest<IEnumerable<User>>;

    public record GetUserByLoginInfoQuery(UserLoginInfo loginInfo) : IRequest<User?>;

    public record GetUserByIdQuery(int Id) : IRequest<User?>;

    #endregion

    #region Commands
    public record AddUserCommand(UserDto userDto) : IRequest<User>;

    public record UpdateUserCommand(int Id, UserDto userDto) : IRequest<User?>;

    public record DeleteUserByIdCommand(int Id) : IRequest<int>;

    public record RestoreUserByIdCommand(int Id) : IRequest<int>;

    public record ConfirmUserEmailCommand(string token) : IRequest<int>;

    public record RequestPasswordRestoreCommand(string email) : IRequest<string>;

    public record RestorePasswordCommand(string email) : IRequest<(string, string)>;

    #endregion

    #region Notifications

    public record UserInfoRequestedNotification(string email, string subject, string content) : INotification;

    public record UserModifiedNotification(User user) : INotification;

    public record UserActiveStatusChangedNotification(int userId, bool isActive) : INotification;

    #endregion
}
