using MediatR;
using InnoShop.Products.API.Models;

namespace InnoShop.Products.API.Abstract.Operations
{
    public abstract class QCHandlerBase
    {
        protected readonly IProductsRepository _productsRepo;
        public QCHandlerBase(IProductsRepository productsRepo) => _productsRepo = productsRepo;
    }

    // auth user
    public class AuthUserHandler : IRequestHandler<AuthUserByLoginInfoQuery, (bool success, UserLoginResponse? response, string? errorMessage)>
    {
        private readonly ILoginProvider _loginProvider;

        public AuthUserHandler(ILoginProvider loginProvider)
        {
            _loginProvider = loginProvider;
        }

        public async Task<(bool success, UserLoginResponse? response, string? errorMessage)>
            Handle(AuthUserByLoginInfoQuery request, CancellationToken cancellationToken)
        {
            var result = await _loginProvider.LoginAsync(request.loginInfo);
            return result;
        }
    }

    // get products
    public class GetProductsHandler : QCHandlerBase, IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        public GetProductsHandler(IProductsRepository productsRepo) : base(productsRepo) { }
        public async Task<IEnumerable<Product>> Handle(GetProductsQuery request,
            CancellationToken cancellationToken) => await _productsRepo.GetProductsAsync(request.userId, request.filter);
    }

    // get product by Id
    public class GetProductByIdHandler : QCHandlerBase, IRequestHandler<GetProductByIdQuery, Product?>
    {
        public GetProductByIdHandler(IProductsRepository productsRepository) : base(productsRepository) { }
        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) =>
            await _productsRepo.GetProductByIdAsync(request.userId, request.productId);
    }
}
