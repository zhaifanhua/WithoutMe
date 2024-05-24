#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:SignalRSetup
// Guid:d6d2aa01-9136-426a-82aa-5a02aaa7b1bc
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:18:15
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// SignalRSetup
/// </summary>
public static class SignalRSetup
{
    /// <summary>
    /// SignalR 服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSignalRSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSignalR(options =>
        {
#if DEBUG
            // 当SignalR连接出现问题时，客户端会收到详细错误信息
            options.EnableDetailedErrors = true;
#endif
            // 客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
            options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
            // 服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
            options.KeepAliveInterval = TimeSpan.FromMinutes(2);
        });

        return services;
    }
}