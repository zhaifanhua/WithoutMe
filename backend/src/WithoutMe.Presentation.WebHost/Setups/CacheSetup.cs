#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:CacheSetup
// Guid:cbfe3f30-bc46-42ee-89df-aeb5aede12af
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 11:04:32
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using WithoutMe.Presentation.WebHost.Options;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// CacheSetup
/// </summary>
public static class CacheSetup
{
    /// <summary>
    /// 缓存服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddCacheSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 内存缓存(默认开启)
        services.AddMemoryCache();
        services.AddSingleton<IAppCacheService, AppCacheService>();

        // 分布式缓存
        var isEnabledRedisCache = AppOptions.Cache.RedisCache.IsEnabled.GetValue();
        if (isEnabledRedisCache)
        {
            // CSRedis
            var connectionString = AppOptions.Cache.RedisCache.ConnectionString.GetValue();
            var prefix = AppOptions.Cache.RedisCache.Prefix.GetValue();
            var redisStr = $"{connectionString}, prefix = {prefix}";
            CSRedisClient redisClient = new(redisStr);
            // 用法一：基于 Redis 初始化 IDistributedCache
            services.AddSingleton(redisClient);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(redisClient));
            // 用法二：帮助类直接调用
            RedisHelper.Initialization(redisClient);
            services.AddDistributedMemoryCache();
        }

        return services;
    }
}