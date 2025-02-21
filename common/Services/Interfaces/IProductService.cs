using common.Entities;
using common.Models.Products;

namespace common.Services.Interfaces
{
    public interface IProductService
	{
        Task<List<ProductDTO>> GetAllProducts();

        Task<ProductDTO> GetProductById(string id);

        Task<string> CreateProduct(ProductReq reqBody);

        Task<bool> UpdateProduct(string id, ProductReq reqBody);

        Task<bool> DeleteProduct(string id);
    }
}

