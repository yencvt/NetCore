using System;
namespace common.Services.Interfaces
{
	public interface IMultipleCacheService
	{
        Task<T?> GetOrSetCacheAsync<T>(string key, Func<Task<T>> getData, TimeSpan expiration);
        Task RemoveCacheAsync(string key);
    }
}

