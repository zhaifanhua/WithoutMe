#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:HttpSetup
// Guid:1064eb84-75ac-4e1b-a277-135df4bbf3de
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 10:51:43
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.HttpOverrides;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// HttpSetup
/// </summary>
public static class HttpSetup
{
    /// <summary>
    /// Http 应用扩展
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseHttpSetup(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(app);

        // 环境变量，开发环境
        if (env.IsDevelopment())
        {
            // 生成异常页面
            app.UseDeveloperExceptionPage();
        }

        // 使用HSTS的中间件，该中间件添加了严格传输安全头
        app.UseHsts();
        // Nginx 反向代理获取真实IP
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        // 强制https跳转
        //app.UseHttpsRedirection();

        return app;
    }
}