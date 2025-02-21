using common.Entities;
using common.Models.Products;
using common.Repositories;
using common.Services.Interfaces;
using common.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace common.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => {
                return JsonUtils.ConvertObjectToObject<ProductDTO>(p);
            }).ToList();
        }

        public async Task<ProductDTO> GetProductById(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return JsonUtils.ConvertObjectToObject<ProductDTO>(product);
        }

        public async Task<string> CreateProduct(ProductReq reqBody)
        {
            ProductEntity product = JsonUtils.ConvertObjectToObject<ProductEntity>(reqBody);

            await _productRepository.CreateAsync(product);
            return product.Id;
        }

        public async Task<bool> UpdateProduct(string id, ProductReq reqBody)
        {
            ProductEntity product = JsonUtils.ConvertObjectToObject<ProductEntity>(reqBody);

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return false;

            product.Id = id;
            await _productRepository.UpdateAsync(id, product);
            return true;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null) return false;

            await _productRepository.DeleteAsync(id);
            return true;
        }
    }
}
