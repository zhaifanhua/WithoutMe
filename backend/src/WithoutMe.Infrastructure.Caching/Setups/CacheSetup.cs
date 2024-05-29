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

using Microsoft.Extensions.DependencyInjection;
using WithoutMe.Infrastructure.Caching.Options;

namespace WithoutMe.Infrastructure.Caching.Setups;

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

        var redisCacheOptions = services.ExecutePreConfiguredActions<RedisCacheOptions>();

        // 内存缓存(默认开启)
        services.AddMemoryCache();

        // 分布式缓存
        if (redisCacheOptions.IsEnabled)
        {
            // CSRedis
            var connectionString = redisCacheOptions.ConnectionString;
            var prefix = redisCacheOptions.Prefix;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = prefix;
            });
        }

        return services;
    }
}