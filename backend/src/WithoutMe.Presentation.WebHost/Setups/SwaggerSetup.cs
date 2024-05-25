#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2022 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:SwaggerSetup
// Guid:40c713e1-7cdf-42da-9e08-84d1bff1489f
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreatedTime:2022-09-29 上午 01:03:07
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Runtime.InteropServices;
using WithoutMe.Presentation.WebHost.Common.Swagger;
using WithoutMe.Presentation.WebHost.Options;
using XiHan.Utils.Hardware;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// SwaggerSetup
/// </summary>
public static class SwaggerSetup
{
    /// <summary>
    /// Swagger 服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var swaggerOptions = services.ExecutePreConfiguredActions<SwaggerOptions>();

        // 配置Swagger，从路由、控制器和模型构建对象
        services.AddSwaggerGen(options =>
        {
            // 配置Swagger文档信息
            SwaggerInfoConfig(options, swaggerOptions);
            // 配置Swagger分组信息
            SwaggerGroupConfig(options);
            // 加载Swagger注释文档
            SwaggerDocLoad(options);
            // 配置Swagger文档请求 带JWT Token
            SwaggerJwtConfig(options);
        });
        return services;
    }

    /// <summary>
    /// Swagger 应用扩展
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // 路由前缀
            var routePrefix = swaggerOptions.RoutePrefix;
            // 需要暴露的分组
            string[] publishGroup = swaggerOptions.PublishGroup;

            // 根据分组遍历展示
            typeof(ApiGroupNameEnum).GetFields().Skip(1).ToList().ForEach(group =>
            {
                // 获取枚举值上的特性
                if (publishGroup.All(pGroup => !string.Equals(pGroup, group.Name, StringComparison.CurrentCultureIgnoreCase))) return;

                var info = group.GetCustomAttributes(typeof(GroupInfoAttribute), false).OfType<GroupInfoAttribute>().FirstOrDefault();
                // 切换分组操作,参数一是使用的哪个json文件,参数二是个名字
                options.SwaggerEndpoint($"/swagger/{group.Name}/swagger.json", info?.Title);
            });

            // 入口程序集
            var entryAssembly = Assembly.GetEntryAssembly()!;
            // 将swagger首页，设置成自定义的页面，写法：{ 项目名.index.html}
            options.IndexStream = () => entryAssembly.GetManifestResourceStream($"{entryAssembly.GetName().Name}.index.html");
            options.HeadContent = @"<style>.opblock-summary-description{font-weight: bold;text-align: right;}</style>";

            // Api页面标题
            options.DocumentTitle = $"无我接口文档";
            // Api文档仅展开标记
            // List：列表式(展开子类)，默认值;
            // Full：完全展开;
            // None：列表式(不展开子类)
            options.DocExpansion(DocExpansion.None);
            // 模型的默认扩展深度，设置为 -1 完全隐藏模型
            options.DefaultModelsExpandDepth(-1);
            // Api前缀设置
            options.RoutePrefix = routePrefix;
        });

        return app;
    }

    /// <summary>
    /// Swagger 文档信息配置
    /// </summary>
    /// <param name="options"></param>
    /// <param name="swaggerOptions"></param>
    private static void SwaggerInfoConfig(SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        // 需要暴露的分组
        string[] publishGroup = swaggerOptions.PublishGroup;
        // 利用枚举反射加载出每个分组的接口文档，Skip(1)是因为Enum第一个FieldInfo 是内置的一个 Int 值
        typeof(ApiGroupNameEnum).GetFields().Skip(1).ToList().ForEach(group =>
        {
            // 获取枚举值上的特性
            if (publishGroup.All(pGroup => !string.Equals(pGroup, group.Name, StringComparison.CurrentCultureIgnoreCase))) return;

            // 获取分组信息
            var info = group.GetCustomAttributes(typeof(GroupInfoAttribute), true).OfType<GroupInfoAttribute>().FirstOrDefault();
            // 添加文档介绍
            options.SwaggerDoc(group.Name, new OpenApiInfo
            {
                Title = info?.Title,
                Version = info?.Version,
                Description = info?.Description + $" Powered by {RuntimeInformation.FrameworkDescription} on {OsPlatformHelper.GetOperatingSystem()}",
                Contact = new OpenApiContact
                {
                    Name = "ZhaiFanhua",
                    Email = "me@zhaifanhua.com",
                    Url = new Uri("https://www.zhaifanhua.com")
                },
                License = new OpenApiLicense
                {
                    Name = "MulanPSL2",
                    Url = new Uri("http://license.coscl.org.cn/MulanPSL2")
                }
            });
            // 根据相对路径排序
            //options.OrderActionsBy(o => o.RelativePath);
        });
    }

    /// <summary>
    /// 配置Swagger分组信息
    /// </summary>
    /// <param name="options"></param>
    private static void SwaggerGroupConfig(SwaggerGenOptions options)
    {
        // 核心逻辑代码，指定分组被加载时回调进入。也就是 swagger 右上角下拉框内的分组加载时，每一个分组加载时都会遍历所有控制器的 action 进入一次这个方法体内，返回 true 就暴露，否则隐藏
        options.DocInclusionPredicate((docName, apiDescription) =>
        {
            // 反射获取基类 BaseApiController 的 ApiGroupAttribute 信息
            List<ApiGroupAttribute>? baseControllerAttributeList = ((ControllerActionDescriptor)apiDescription.ActionDescriptor).ControllerTypeInfo.BaseType?
                .GetCustomAttributes(typeof(ApiGroupAttribute), true).OfType<ApiGroupAttribute>()
                .ToList();
            // 反射获取 ApiController 的 ApiGroupAttribute 信息
            List<ApiGroupAttribute> controllerAttributeList = ((ControllerActionDescriptor)apiDescription.ActionDescriptor)
                .ControllerTypeInfo
                .GetCustomAttributes(typeof(ApiGroupAttribute), true).OfType<ApiGroupAttribute>()
                .ToList();
            // 反射获取派生类 Action 的 ApiGroupAttribute 信息
            List<ApiGroupAttribute> actionAttributeList = apiDescription.ActionDescriptor.EndpointMetadata
                .Where(x => x is ApiGroupAttribute).OfType<ApiGroupAttribute>()
                .ToList();

            // 所有含 ApiGroupAttribute 的集合
            List<ApiGroupAttribute> apiGroupAttributeList = [];
            // 为空时插入空，减少 if 判断
            List<ApiGroupAttribute> emptyAttribute = [];
            apiGroupAttributeList.AddRange(baseControllerAttributeList ?? emptyAttribute);
            apiGroupAttributeList.AddRange(controllerAttributeList);
            apiGroupAttributeList.AddRange(actionAttributeList);

            // 判断所有的分组名称是否含有此名称
            if (apiGroupAttributeList.Count != 0)
            {
                List<bool> containList = [];
                // 遍历判断是否包含这个分组
                apiGroupAttributeList.ForEach(attribute =>
                {
                    containList.Add(attribute.GroupNames.Any(x => x.ToString() == docName));
                });
                // 若有，则为该分组名称分配此 Action
                if (containList.Any(c => c)) return true;
            }

            return false;
        });
    }

    /// <summary>
    /// 加载Swagger注释文档
    /// </summary>
    /// <param name="options"></param>
    private static void SwaggerDocLoad(SwaggerGenOptions options)
    {
        // 枚举添加摘要
        options.UseInlineDefinitionsForEnums();

        try
        {
            // 生成注释文档，必须在 OperationFilter<AppendAuthorizeToSummaryOperationFilter>() 之前，否则没有(Auth)标签
            Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(xmlPath =>
            {
                // 默认的第二个参数是false，这个是 controller 的注释，true 时会显示注释，否则只显示方法注释
                options.IncludeXmlComments(xmlPath, true);
            });
        }
        catch (Exception ex)
        {
            var errorMsg = $"Swagger 文档加载失败！";
            throw new Exception(errorMsg, ex);
        }
    }

    /// <summary>
    /// Swagger 文档中请求带 JWT Token
    /// </summary>
    /// <param name="options"></param>
    private static void SwaggerJwtConfig(SwaggerGenOptions options)
    {
        // 定义安全方案
        OpenApiSecurityScheme securitySchemeOAuth2 = new()
        {
            // JWT默认的参数名称
            Name = "Authorization",
            // 描述
            Description = "在下框中输入<code>token</code>进行身份验证",
            // 标识承载令牌的Bearer认证的数据格式，该信息主要是用于文档
            BearerFormat = "JWT",
            // 认证主题，在Type=Http时，只能是Basic和Bearer
            Scheme = "Bearer",
            // 表示认证信息发在Http请求的哪个位置
            In = ParameterLocation.Header,
            // 表示认证方式，有ApiKey，Http，OAuth2，OpenIdConnect四种，其中ApiKey是用的最多的
            Type = SecuritySchemeType.Http
        };

        // 定义认证，方案名称必须是 oauth2
        options.AddSecurityDefinition("oauth2", securitySchemeOAuth2);
        // 注册全局认证，即所有的接口都可以使用认证
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                // 必须与上面声明的一致，否则小绿锁混乱,即API全部会加小绿锁
                securitySchemeOAuth2,
                Array.Empty<string>()
            }
        });

        // 文档中显示安全小绿锁，在对应的 Action 上添加[Authorize]
        options.OperationFilter<AddResponseHeadersFilter>();
        // 安全小绿锁旁标记 Auth 标签
        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        // 添加请求头的 Header 中的 token,传递到后台
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
}