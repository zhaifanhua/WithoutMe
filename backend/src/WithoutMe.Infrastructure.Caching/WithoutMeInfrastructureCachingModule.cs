#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeInfrastructureCachingModule
// Guid:66adf1ef-2002-4597-bccc-aa2e23169115
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-28 下午 05:02:27
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using WithoutMe.Infrastructure.Caching.Options;
using WithoutMe.Infrastructure.Caching.Setups;

namespace WithoutMe.Infrastructure.Caching;

/// <summary>
/// 无我基础设施缓存模块
/// </summary>
public class WithoutMeInfrastructureCachingModule : AbpModule
{
    /// <summary>
    /// 预配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<RedisCacheOptions>(options =>
        {
            var redisCache = configuration.GetSection("RedisCache");

            options.IsEnabled = redisCache.GetValue<bool>(nameof(options.IsEnabled));
            options.ConnectionString = redisCache.GetValue<string>(nameof(options.ConnectionString)) ?? string.Empty;
            options.Prefix = redisCache.GetValue<string>(nameof(options.Prefix)) ?? string.Empty;
            options.SyncTimeout = redisCache.GetValue<int>(nameof(options.SyncTimeout));
        });
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        // 缓存
        services.AddCacheSetup();
    }
}