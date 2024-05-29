#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ResponseCacheSetup
// Guid:36dbee99-f16e-45e4-8d2b-b640fa97d202
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:19:00
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.Options;
using WithoutMe.Presentation.WebHost.Options;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// ResponseCacheSetup
/// </summary>
public static class ResponseCacheSetup
{
    /// <summary>
    /// 响应缓存 服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddResponseCacheSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 响应缓存
        services.AddResponseCaching(options =>
        {
            // 确定是否将响应缓存在区分大小写的路径上。 默认值是 false
            options.UseCaseSensitivePaths = false;
            // 响应正文的最大可缓存大小(以字节为单位)。 默认值为 64 * 1024 * 1024 (64 MB)
            options.MaximumBodySize = 2 * 1024 * 1024;
            // 响应缓存中间件的大小限制(以字节为单位)。 默认值为 100 * 1024 * 1024 (100 MB)
            options.SizeLimit = 100 * 1024 * 1024;
        });

        return services;
    }
}