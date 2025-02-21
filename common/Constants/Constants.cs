using System;
namespace common.Constants
{
    public static class Constants
    {
        // 🔹 Thời gian cache mặc định
        public const int DefaultCacheDurationSeconds = 3600; // 1 giờ
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(DefaultCacheDurationSeconds);

        // 🔹 Khóa cache (Key Prefix)
        public const string CachePrefix = "APP_CACHE_";
        public const string UserCacheKey = CachePrefix + "USER_{0}";
        public const string ProductCacheKey = CachePrefix + "PRODUCT_{0}";

        // 🔹 Cấu hình Redis
        public const string RedisConnectionString = "localhost:6379";
        public const int RedisTimeoutMilliseconds = 3000;

        // 🔹 Cấu hình Logging
        public const string LogFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

        // 🔹 API Endpoints
        public const string ApiBaseUrl = "https://api.example.com";
        public const string UserApiEndpoint = ApiBaseUrl + "/users";
        public const string ProductApiEndpoint = ApiBaseUrl + "/products";
    }
}

