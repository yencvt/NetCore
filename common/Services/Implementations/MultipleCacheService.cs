using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using common.Services.Interfaces;
using System.Text.Json;
using common.Models.Configs;
using Microsoft.Extensions.Options;

namespace common.Cacher
{
    public class MultipleCacheService : IMultipleCacheService
    {
        private readonly IMemoryCache? _memoryCache;
        private readonly IDistributedCache? _distributedCache;
        private readonly ILogger<MultipleCacheService> _logger;
        private readonly CacheConfig _cacheConfig;
        private readonly TimeSpan _redisTimeout;

        public MultipleCacheService(
            ILogger<MultipleCacheService> logger,
            IOptions<CacheConfig> cacheConfig,
            IMemoryCache? memoryCache = null,
            IDistributedCache? distributedCache = null)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _logger = logger;
            _cacheConfig = cacheConfig.Value;
            _redisTimeout = TimeSpan.FromSeconds(_cacheConfig.RedisTimeout);
        }

        // <summary>
        /// Lấy dữ liệu từ cache (MemoryCache hoặc Redis), nếu không có thì gọi `getData` để lấy từ nguồn khác.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần cache.</typeparam>
        /// <param name="key">Khóa của cache.</param>
        /// <param name="getData">Hàm để lấy dữ liệu nếu cache không có.</param>
        /// <param name="expiration">Thời gian hết hạn của cache.</param>
        /// <returns>Dữ liệu đã lấy từ cache hoặc nguồn dữ liệu.</returns>
        public async Task<T?> GetOrSetCacheAsync<T>(string key, Func<Task<T>> getData, TimeSpan expiration)
        {
            T? cacheData = default;

            // 1. Kiểm tra MemoryCache trước
            if (_cacheConfig.EnableMemoryCache && _memoryCache.TryGetValue(key, out cacheData))
            {
                _logger.LogInformation($"MemoryCache hit: {key}");
                return cacheData;
            }

            // 2. Kiểm tra RedisCache nếu được bật
            if (_cacheConfig.EnableRedisCache)
            {
                try
                {
                    var redisTask = _distributedCache.GetAsync(key);

                    // Chờ phản hồi từ Redis, nếu quá timeout thì bỏ qua Redis
                    if (await Task.WhenAny(redisTask, Task.Delay(_redisTimeout)) == redisTask)
                    {
                        var redisData = redisTask.Result;
                        if (redisData != null)
                        {
                            cacheData = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(redisData));

                            // Nếu dùng MemoryCache, lưu lại để truy vấn nhanh hơn
                            if (_cacheConfig.EnableMemoryCache && cacheData != null)
                            {
                                _memoryCache.Set(key, cacheData, expiration);
                            }

                            return cacheData;
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Redis timeout for key: {key}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Redis error: {ex.Message}");
                }
            }

            // 3. Nếu không tìm thấy trong cache, lấy dữ liệu từ `getData`
            cacheData = await getData();
            if (cacheData != null)
            {
                // Lưu vào MemoryCache nếu được bật
                if (_cacheConfig.EnableMemoryCache)
                {
                    _memoryCache.Set(key, cacheData, expiration);
                }

                // Lưu vào RedisCache nếu được bật
                if (_cacheConfig.EnableRedisCache)
                {
                    try
                    {
                        var jsonData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cacheData));
                        var cacheTask = _distributedCache.SetAsync(key, jsonData, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = expiration
                        });

                        // Chờ lưu Redis, nếu quá timeout thì bỏ qua lỗi
                        if (await Task.WhenAny(cacheTask, Task.Delay(_redisTimeout)) != cacheTask)
                        {
                            _logger.LogWarning($"Redis set timeout for key: {key}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to save to Redis: {ex.Message}");
                    }
                }
            }

            return cacheData;
        }

        //// <summary>
        /// Lưu dữ liệu vào cache với thời gian hết hạn tùy chỉnh.
        /// Nếu Redis gặp lỗi hoặc phản hồi quá chậm, hệ thống vẫn tiếp tục hoạt động bình thường.
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu cần lưu vào cache.</typeparam>
        /// <param name="key">Khóa của cache.</param>
        /// <param name="value">Dữ liệu cần lưu trữ.</param>
        /// <param name="expiration">Thời gian hết hạn của cache.</param>
        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            if (_cacheConfig.EnableMemoryCache)
            {
                _memoryCache.Set(key, value, expiration);
                _logger.LogInformation($"Saved to MemoryCache: {key}");
            }

            if (_cacheConfig.EnableRedisCache)
            {
                try
                {
                    var jsonData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
                    var cacheTask = _distributedCache.SetAsync(key, jsonData, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expiration
                    });

                    // Giới hạn thời gian lưu Redis, nếu chậm quá thì bỏ qua
                    if (await Task.WhenAny(cacheTask, Task.Delay(_redisTimeout)) != cacheTask)
                    {
                        _logger.LogWarning($"Redis set cache timeout for key: {key}");
                    }
                    else
                    {
                        _logger.LogInformation($"Saved to RedisCache: {key}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to save to Redis: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu cache theo key.
        /// Nếu Redis gặp lỗi hoặc phản hồi quá chậm, hệ thống vẫn tiếp tục hoạt động bình thường.
        /// </summary>
        /// <param name="key">Khóa của cache cần xóa.</param>
        public async Task RemoveCacheAsync(string key)
        {
            if (_cacheConfig.EnableMemoryCache)
            {
                _memoryCache.Remove(key);
                _logger.LogInformation($"Removed from MemoryCache: {key}");
            }

            if (_cacheConfig.EnableRedisCache)
            {
                try
                {
                    var removeTask = _distributedCache.RemoveAsync(key);

                    // Giới hạn thời gian xóa Redis, nếu chậm quá thì bỏ qua
                    if (await Task.WhenAny(removeTask, Task.Delay(_redisTimeout)) != removeTask)
                    {
                        _logger.LogWarning($"Redis remove cache timeout for key: {key}");
                    }
                    else
                    {
                        _logger.LogInformation($"Removed from RedisCache: {key}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to remove from Redis: {ex.Message}");
                }
            }
        }
    }
}
