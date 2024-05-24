#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:RouteSetup
// Guid:e1cbda29-4476-4c52-83c5-ab6217d89987
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:17:15
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// RouteSetup
/// </summary>
public static class RouteSetup
{
    /// <summary>
    /// Route 服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddRouteSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddRouting(route =>
        {
            route.LowercaseUrls = false;
            route.LowercaseQueryStrings = false;
            // 路由前后加斜杠 /
            route.AppendTrailingSlash = false;
        });
        return services;
    }
}