using System;
using common.Entities;
using common.Services.Interfaces;
using MongoDB.Driver;

namespace common.Repositories
{
	public class ProductRepository
	{
        private readonly IUserService _userService;
        private readonly IMongoCollection<ProductEntity> _products;

        public ProductRepository(IMongoDatabase database, IUserService userService)
        {
            _products = database.GetCollection<ProductEntity>("products");
            _userService = userService;
        }

        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<ProductEntity> GetByIdAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(ProductEntity product)
        {
            product.CreatedBy = _userService.GetCurrentUserId();
            product.UpdatedBy = product.CreatedBy;
            product.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            product.UpdatedAt = product.CreatedAt;

            await _products.InsertOneAsync(product);
        }

        public async Task UpdateAsync(string id, ProductEntity product)
        {
            product.UpdatedBy = _userService.GetCurrentUserId();
            product.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await _products.ReplaceOneAsync(p => p.Id == id, product);
        }

        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }
    }
}

