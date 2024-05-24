#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMePresentationWebHostModule
// Guid:5173ceed-d177-473d-be60-ab223b77ff81
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 03:09:44
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.HttpOverrides;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using WithoutMe.Application;
using WithoutMe.Presentation.WebHost.Options;
using WithoutMe.Presentation.WebHost.Setups;
using WithoutMe.Presentation.WebHost.Setups.Apps;
using SwaggerOptions = WithoutMe.Presentation.WebHost.Options.SwaggerOptions;

namespace WithoutMe.Presentation.WebHost;

/// <summary>
/// 无我界面主机模块
/// </summary>
[DependsOn(
    typeof(WithoutMeApplicationModule),
    typeof(AbpSwashbuckleModule)
    )]
public class WithoutMePresentationWebHostModule : AbpModule
{
    /// <summary>
    /// 预配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        ArgumentNullException.ThrowIfNull(configuration);

        var cors = configuration.GetSection("Cors");
        var swagger = configuration.GetSection("Swagger");

        PreConfigure<CorsOptions>(preConfigOptions =>
        {
            preConfigOptions.IsEnabled = cors.GetValue<bool>(nameof(preConfigOptions.IsEnabled));
            preConfigOptions.PolicyName = cors.GetValue<string>(nameof(preConfigOptions.PolicyName)) ?? string.Empty;
            preConfigOptions.Origins = cors.GetSection(nameof(preConfigOptions.Origins)).Get<string[]>() ?? [];
            preConfigOptions.Headers = cors.GetSection(nameof(preConfigOptions.Headers)).Get<string[]>() ?? [];
        });
        PreConfigure<SwaggerOptions>(preConfigOptions =>
        {
            preConfigOptions.RoutePrefix = swagger.GetValue<string>(nameof(preConfigOptions.RoutePrefix)) ?? string.Empty;
            preConfigOptions.PublishGroup = swagger.GetSection(nameof(preConfigOptions.PublishGroup)).Get<string[]>() ?? [];
        });
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;

        // 对象映射
        services.AddMapsterSetup();
        // 缓存
        services.AddCacheSetup();
        // SqlSugar
        services.AddSqlSugarSetup();
        // Http上下文
        services.AddHttpPollySetup();
        // 性能分析
        services.AddMiniProfilerSetup();
        // 接口文档
        services.AddSwaggerSetup();
        // 路由
        services.AddRouteSetup();
        // 限流
        services.AddRateLimiterSetup();
        // 跨域
        services.AddCorsSetup();
        // 鉴权授权
        services.AddAuthSetup();
        // Controllers
        services.AddControllersSetup();
        // RabbitMQ
        services.AddRabbitMqSetup();
        // 即时通讯
        services.AddSignalRSetup();
        // 健康检查
        services.AddHealthChecks();
        // 响应缓存
        services.AddResponseCacheSetup();
        // 终端
        services.AddEndpointsApiExplorer();
        // 任务队列
        services.AddJobSetup();
    }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        // 环境变量，开发环境
        if (env.IsDevelopment())
        {
            // 生成异常页面
            app.UseDeveloperExceptionPage();
        }

        // 使用HSTS的中间件，该中间件添加了严格传输安全头
        app.UseHsts();

        // 转发将标头代理到当前请求，配合 Nginx 使用，获取用户真实IP
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        // 路由
        app.UseRouting();

        // 跨域
        app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        // 身份验证
        app.UseAuthentication();

        // 认证授权
        app.UseAuthorization();

        // 路由映射
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}