using InnoShop.Users.API.Models;
using MediatR;

namespace InnoShop.Users.API.Abstract
{
    // add user
    public class AddUserHandler : QCHandlerBase, IRequestHandler<AddUserCommand, User>
    {
        public AddUserHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<User> Handle(AddUserCommand request,
            CancellationToken cancellationToken) => await _usersRepo.AddUserAsync(request.userDto);
    }

    // update user
    public class UpdateUserHandler : QCHandlerBase, IRequestHandler<UpdateUserCommand, User?>
    {
        public UpdateUserHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<User?> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken) => await _usersRepo.UpdateUserAsync(request.Id, request.userDto);
    }

    // delete user by Id
    public class DeleteUserByIdHandler : QCHandlerBase, IRequestHandler<DeleteUserByIdCommand, int>
    {
        public DeleteUserByIdHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<int> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken) =>
            await _usersRepo.DeleteUserAsync(request.Id);
    }

    // restore user by Id
    public class RestoreUserByIdHandler : QCHandlerBase, IRequestHandler<RestoreUserByIdCommand, int>
    {
        public RestoreUserByIdHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<int> Handle(RestoreUserByIdCommand request, CancellationToken cancellationToken) =>
            await _usersRepo.RestoreUserAsync(request.Id);
    }

    // confirm user email
    public class ConfirmUserEmailHandler : QCHandlerBase, IRequestHandler<ConfirmUserEmailCommand, int>
    {
        public ConfirmUserEmailHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<int> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken) =>
            await _usersRepo.ConfirmUserEmailAsync(request.token);
    }

    // restore password request
    public class RequestPasswordRestoreHandler : QCHandlerBase, IRequestHandler<RequestPasswordRestoreCommand, string>
    {
        public RequestPasswordRestoreHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<string> Handle(RequestPasswordRestoreCommand request, CancellationToken cancellationToken) =>
            await _usersRepo.RequestPasswordRestoreAsync(request.email);
    }

    // password restore
    public class RestorePasswordHandler : QCHandlerBase, IRequestHandler<RestorePasswordCommand, (string, string)>
    {
        public RestorePasswordHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<(string, string)> Handle(RestorePasswordCommand request, CancellationToken cancellationToken) =>
            await _usersRepo.RestorePasswordAsync(request.email);
    }
}
