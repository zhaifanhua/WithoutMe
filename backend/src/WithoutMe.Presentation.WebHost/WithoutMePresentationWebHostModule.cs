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

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using WithoutMe.Presentation.HttpApi;
using WithoutMe.Presentation.WebHost.Handlers;
using WithoutMe.Presentation.WebHost.Options;
using WithoutMe.Presentation.WebHost.Options.Authorize;
using WithoutMe.Presentation.WebHost.Setups;
using SwaggerOptions = WithoutMe.Presentation.WebHost.Options.SwaggerOptions;

namespace WithoutMe.Presentation.WebHost;

/// <summary>
/// 无我界面主机模块
/// </summary>
[DependsOn(
    typeof(WithoutMePresentationHttpApiModule),
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

        var appOptions = new AppOptions();

        PreConfigure<WebHostOptions>(options =>
        {
            var webHost = configuration.GetSection("WebHost");

            options.IsDemoMode = webHost.GetValue<bool>(nameof(options.IsDemoMode));

            appOptions.WebHostOptions = options;
        });
        PreConfigure<CorsOptions>(options =>
        {
            var cors = configuration.GetSection("Cors");

            options.IsEnabled = cors.GetValue<bool>(nameof(options.IsEnabled));
            options.PolicyName = cors.GetValue<string>(nameof(options.PolicyName)) ?? string.Empty;
            options.Origins = cors.GetSection(nameof(options.Origins)).Get<string[]>() ?? [];
            options.Headers = cors.GetSection(nameof(options.Headers)).Get<string[]>() ?? [];

            appOptions.CorsOptions = options;
        });
        PreConfigure<SwaggerOptions>(options =>
        {
            var swagger = configuration.GetSection("Swagger");

            options.RoutePrefix = swagger.GetValue<string>(nameof(options.RoutePrefix)) ?? string.Empty;
            options.PublishGroup = swagger.GetSection(nameof(options.PublishGroup)).Get<string[]>() ?? [];

            appOptions.SwaggerOptions = options;
        });
        PreConfigure<MiniprofilerOptions>(options =>
        {
            var miniProfiler = configuration.GetSection("MiniProfiler");

            options.IsEnabled = miniProfiler.GetValue<bool>(nameof(options.IsEnabled));

            appOptions.MiniprofilerOptions = options;
        });
        PreConfigure<JwtOptions>(options =>
        {
            var jwt = configuration.GetSection("Jwt");

            options.Issuer = jwt.GetValue<string>(nameof(options.Issuer)) ?? string.Empty;
            options.Audience = jwt.GetValue<string>(nameof(options.Audience)) ?? string.Empty;
            options.SymmetricKey = jwt.GetValue<string>(nameof(options.SymmetricKey)) ?? string.Empty;
            options.Expires = jwt.GetValue<int>(nameof(options.Expires));
            options.ClockSkew = jwt.GetValue<int>(nameof(options.ClockSkew));

            appOptions.JwtOptions = options;
        });
        PreConfigure<AuthOptions>(options =>
        {
            var authOption = configuration.GetSection("Auth");
            var githubOption = authOption.GetSection("Github");
            var giteeOption = authOption.GetSection("Gitee");
            var alipayOption = authOption.GetSection("Alipay");
            var dingtalkOption = authOption.GetSection("Dingtalk");
            var microsoftOption = authOption.GetSection("Microsoft");
            var weiboOptions = authOption.GetSection("Weibo");
            var qqOptions = authOption.GetSection("QQ");

            options.Github = new GithubOptions
            {
                ClientId = githubOption.GetValue<string>(nameof(options.Github.ClientId)) ?? string.Empty,
                ClientSecret = githubOption.GetValue<string>(nameof(options.Github.ClientSecret)) ?? string.Empty,
                RedirectUrl = githubOption.GetValue<string>(nameof(options.Github.RedirectUrl)) ?? string.Empty,
                Scope = githubOption.GetValue<string>(nameof(options.Github.Scope)) ?? string.Empty
            };
            options.Gitee = new GiteeOptions
            {
                ClientId = giteeOption.GetValue<string>(nameof(options.Gitee.ClientId)) ?? string.Empty,
                ClientSecret = giteeOption.GetValue<string>(nameof(options.Gitee.ClientSecret)) ?? string.Empty,
                RedirectUrl = giteeOption.GetValue<string>(nameof(options.Gitee.RedirectUrl)) ?? string.Empty,
                Scope = giteeOption.GetValue<string>(nameof(options.Gitee.Scope)) ?? string.Empty
            };
            options.Alipay = new AlipayOptions
            {
                AppId = alipayOption.GetValue<string>(nameof(options.Alipay.AppId)) ?? string.Empty,
                RedirectUrl = alipayOption.GetValue<string>(nameof(options.Alipay.RedirectUrl)) ?? string.Empty,
                Scope = alipayOption.GetValue<string>(nameof(options.Alipay.Scope)) ?? string.Empty,
                PrivateKey = alipayOption.GetValue<string>(nameof(options.Alipay.PrivateKey)) ?? string.Empty,
                PublicKey = alipayOption.GetValue<string>(nameof(options.Alipay.PublicKey)) ?? string.Empty
            };
            options.Dingtalk = new DingtalkOptions
            {
                AppId = dingtalkOption.GetValue<string>(nameof(options.Dingtalk.AppId)) ?? string.Empty,
                AppSecret = dingtalkOption.GetValue<string>(nameof(options.Dingtalk.AppSecret)) ?? string.Empty,
                RedirectUrl = dingtalkOption.GetValue<string>(nameof(options.Dingtalk.RedirectUrl)) ?? string.Empty,
                Scope = dingtalkOption.GetValue<string>(nameof(options.Dingtalk.Scope)) ?? string.Empty
            };
            options.Microsoft = new MicrosoftOptions
            {
                ClientId = microsoftOption.GetValue<string>(nameof(options.Microsoft.ClientId)) ?? string.Empty,
                ClientSecret = microsoftOption.GetValue<string>(nameof(options.Microsoft.ClientSecret)) ?? string.Empty,
                RedirectUrl = microsoftOption.GetValue<string>(nameof(options.Microsoft.RedirectUrl)) ?? string.Empty,
                Scope = microsoftOption.GetValue<string>(nameof(options.Microsoft.Scope)) ?? string.Empty
            };
            options.Weibo = new WeiboOptions
            {
                ClientId = weiboOptions.GetValue<string>(nameof(options.Weibo.ClientId)) ?? string.Empty,
                ClientSecret = weiboOptions.GetValue<string>(nameof(options.Weibo.ClientSecret)) ?? string.Empty,
                RedirectUrl = weiboOptions.GetValue<string>(nameof(options.Weibo.RedirectUrl)) ?? string.Empty,
                Scope = weiboOptions.GetValue<string>(nameof(options.Weibo.Scope)) ?? string.Empty
            };
            options.QQ = new QQOptions
            {
                ClientId = qqOptions.GetValue<string>(nameof(options.QQ.ClientId)) ?? string.Empty,
                ClientSecret = qqOptions.GetValue<string>(nameof(options.QQ.ClientSecret)) ?? string.Empty,
                RedirectUrl = qqOptions.GetValue<string>(nameof(options.QQ.RedirectUrl)) ?? string.Empty,
                Scope = qqOptions.GetValue<string>(nameof(options.QQ.Scope)) ?? string.Empty
            };

            appOptions.AuthOptions = options;
        });

        PreConfigure<AppOptions>(options =>
        {
            options = appOptions;
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
        // SqlSugar
        //services.AddSqlSugarSetup();
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
        //services.AddRabbitMqSetup();
        // 即时通讯
        services.AddSignalRSetup();
        // 健康检查
        services.AddHealthChecks();
        // 终端
        services.AddEndpointsApiExplorer();
        // 任务队列
        //services.AddJobSetup();
    }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        // MiniProfiler
        app.UseMiniProfilerSetup();
        // Http
        app.UseHttpSetup(env);
        // WebSocket支持，SignalR优先使用WebSocket传输
        app.UseWebSockets();
        // Swagger
        app.UseSwaggerSetup();
        // 静态文件，访问 wwwroot 目录文件，必须在 UseRouting 之前
        app.UseStaticFiles();
        // 路由
        app.UseRouting();
        // 限流，若作用于特定路由，必须在 UseRouting 之后
        app.UseRateLimiter();
        // 跨域，必须在 UseRouting 之后、UseEndpoints 之前添加
        app.UseCorsSetup();
        // 身份验证
        app.UseAuthentication();
        // 认证授权
        app.UseAuthorization();
        // 响应缓存
        app.UseResponseCaching();
        // 全局日志中间件
        //app.UseMiddleware<GlobalLogMiddleware>();
        // 路由映射
        app.UseEndpoints(endpoints =>
        {
            // 不对约定路由做任何假设，也就是不使用约定路由，依赖用户的特性路由
            endpoints.MapControllers();
            // 健康检查
            endpoints.MapHealthChecks("/Healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = WriteResponseHandler.WriteResponse
            });
            // 即时通讯集线器
            //endpoints.MapHub<ChatHub>("/ChatHub");
        });
        // 数据库初始化
        //app.UseInitDbSetup();
        // 恢复或启动任务
        //app.UseJobSetup();
    }
}