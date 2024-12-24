using InnoShop.Products.API.Abstract.Operations;
using InnoShop.Products.API.Models;
using MediatR;

namespace InnoShop.Products.API.Abstract
{
    // add product
    public class AddProductHandler : QCHandlerBase, IRequestHandler<AddProductCommand, Product>
    {
        public AddProductHandler(IProductsRepository prodRepo) : base(prodRepo) { }
        public async Task<Product> Handle(AddProductCommand request,
            CancellationToken cancellationToken) => await _productsRepo.AddProductAsync(request.userId, request.productDto);
    }

    // update product
    public class UpdateProductHandler : QCHandlerBase, IRequestHandler<UpdateProductCommand, Product?>
    {
        public UpdateProductHandler(IProductsRepository prodRepo) : base(prodRepo) { }
        public async Task<Product?> Handle(UpdateProductCommand request,
            CancellationToken cancellationToken) =>
            await _productsRepo.UpdateProductAsync(request.userId, request.productId, request.productDto);
    }

    // delete product by Id
    public class DeleteProductByIdHandler : QCHandlerBase, IRequestHandler<DeleteProductCommand, int>
    {
        public DeleteProductByIdHandler(IProductsRepository productsRepo) : base(productsRepo) { }
        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken) =>
            await _productsRepo.DeleteProductAsync(request.userId, request.productId);
    }

    // restore product by Id
    public class RestoreProductByIdHandler : QCHandlerBase, IRequestHandler<RestoreProductCommand, int>
    {
        public RestoreProductByIdHandler(IProductsRepository prodRepo) : base(prodRepo) { }
        public async Task<int> Handle(RestoreProductCommand request, CancellationToken cancellationToken) =>
            await _productsRepo.RestoreProductAsync(request.userId, request.productId);
    }

    // deactivate products by user Id
    public class DeactivateProductsByUserIdHandler : QCHandlerBase, IRequestHandler<DeactivateProductsByUserIdCommand, int>
    {
        public DeactivateProductsByUserIdHandler(IProductsRepository prodRepo) : base(prodRepo) { }
        public async Task<int> Handle(DeactivateProductsByUserIdCommand request, CancellationToken cancellationToken) =>
            await _productsRepo.DeactivateProductsByUserIdAsync(request.userId);
    }

    // activate products by user Id
    public class ActivateProductsByUserIdHandler : QCHandlerBase, IRequestHandler<ActivateProductsByUserIdCommand, int>
    {
        public ActivateProductsByUserIdHandler(IProductsRepository prodRepo) : base(prodRepo) { }
        public async Task<int> Handle(ActivateProductsByUserIdCommand request, CancellationToken cancellationToken) =>
            await _productsRepo.ActivateProductsByUserIdAsync(request.userId);
    }

}
