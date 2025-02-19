using common.Entities;

namespace common.Services.Interfaces
{
    public interface IProductService
	{
        Task<List<ProductEntity>> GetAllProducts();

        Task<ProductEntity> GetProductById(string id);

        Task CreateProduct(ProductEntity product);

        Task UpdateProduct(string id, ProductEntity product);

        Task DeleteProduct(string id);
    }
}

