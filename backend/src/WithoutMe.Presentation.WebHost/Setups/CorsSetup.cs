#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:CorsSetup
// Guid:531030f7-533d-4b12-815f-20e60671a0fd
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:06:26
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.Options;
using WithoutMe.Presentation.WebHost.Options;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// CorsSetup
/// </summary>
public static class CorsSetup
{
    /// <summary>
    /// 跨源资源共享服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddCorsSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var corsOptions = services.ExecutePreConfiguredActions<CorsOptions>();

        var isEnabledCors = corsOptions.IsEnabled;
        if (!isEnabledCors) return services;

        services.AddCors(options =>
        {
            // 策略名称
            var policyName = corsOptions.PolicyName;
            // 支持多个域名端口，端口号后不可带/符号
            string[] origins = corsOptions.Origins;
            // 支持多个请求头
            string[] headers = corsOptions.Headers;
            // 添加指定策略
            options.AddPolicy(policyName, policy =>
            {
                // 配置允许访问的域名
                policy.WithOrigins(origins)
                    // 是否允许同源时匹配配置的通配符域
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    // 允许任何请求头
                    .AllowAnyHeader()
                    // 允许任何方法
                    .AllowAnyMethod()
                    // 允许凭据(cookie)
                    .AllowCredentials()
                    // 允许请求头 (SignalR 用此请求头)
                    .WithExposedHeaders(headers);
            });
        });
        return services;
    }

    /// <summary>
    /// 跨源资源共享应用扩展
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseCorsSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var corsOptions = app.ApplicationServices.GetRequiredService<IOptions<CorsOptions>>().Value;

        var isEnabledCors = corsOptions.IsEnabled;
        if (!isEnabledCors) return app;
        // 策略名称
        var policyName = corsOptions.PolicyName;
        // 对 UseCors 的调用必须放在 UseRouting 之后，但在 UseAuthorization 之前
        app.UseCors(policyName);
        return app;
    }
}