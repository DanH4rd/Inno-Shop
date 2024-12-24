using InnoShop.Users.API.Models;
using MediatR;

namespace InnoShop.Users.API.Abstract
{
    public abstract class QCHandlerBase
    {
        protected readonly IUsersRepository _usersRepo;
        public QCHandlerBase(IUsersRepository usersRepo) => _usersRepo = usersRepo;
    }

    // get all users
    public class GetUsersHandler : QCHandlerBase, IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        public GetUsersHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<IEnumerable<User>> Handle(GetUsersQuery request,
            CancellationToken cancellationToken) => await _usersRepo.GetAllUsersAsync();
    }

    // get user by login info
    public class GetUserByLoginInfoHandler : QCHandlerBase, IRequestHandler<GetUserByLoginInfoQuery, User?>
    {
        public GetUserByLoginInfoHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<User?> Handle(GetUserByLoginInfoQuery request, CancellationToken cancellationToken) =>
            await _usersRepo.GetUserByLoginInfoAsync(request.loginInfo);
    }

    // get user by Id
    public class GetUserByIdHandler : QCHandlerBase, IRequestHandler<GetUserByIdQuery, User?>
    {
        public GetUserByIdHandler(IUsersRepository usersRepo) : base(usersRepo) { }
        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) =>
            await _usersRepo.GetUserByIdAsync(request.Id);
    }
}
