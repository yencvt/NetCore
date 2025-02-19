using System;
using common.Entities;
using common.Services.Interfaces;

namespace common.Services.Implementations
{
	public class ProductService : IProductService
	{
		public ProductService()
		{
		}

        public async Task CreateProduct(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProduct(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductEntity>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductEntity> GetProductById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProduct(string id, ProductEntity product)
        {
            throw new NotImplementedException();
        }
    }
}

