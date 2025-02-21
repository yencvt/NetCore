using System;
namespace common.Models.Configs
{
	public class CacheConfig
	{
        public bool EnableMemoryCache { get; set; } = true;
        public bool EnableRedisCache { get; set; } = true;
        public int RedisTimeout { get; set; } = 1;
    }
}

