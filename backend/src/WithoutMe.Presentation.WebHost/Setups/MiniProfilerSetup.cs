#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:MiniProfilerSetup
// Guid:9a69942a-304e-4587-b66c-92d158e541bd
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:24:59
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.Options;
using WithoutMe.Presentation.WebHost.Options;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// MiniProfilerSetup
/// </summary>
public static class MiniProfilerSetup
{
    /// <summary>
    /// MiniProfiler 扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddMiniProfilerSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var miniprofilerOptions = services.ExecutePreConfiguredActions<MiniprofilerOptions>();

        if (!miniprofilerOptions.IsEnabled) return services;

        services.AddMiniProfiler(options =>
        {
            // 指定 MiniProfiler 的路由基础路径
            options.RouteBasePath = @"/Profiler";
            // 指定 MiniProfiler 的颜色方案
            options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
            // 指定 MiniProfiler 弹出窗口的位置
            options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.Left;
            // 指定是否在 MiniProfiler 弹出窗口中显示子操作的执行时间
            options.PopupShowTimeWithChildren = true;
            // 指定是否在 MiniProfiler 弹出窗口中显示执行时间很短的操作
            options.PopupShowTrivial = true;
            // 指定 SQL 查询语句格式化器
            options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
            // 控制是否跟踪数据库连接的打开和关闭操作
            options.TrackConnectionOpenClose = true;
        });

        return services;
    }

    /// <summary>
    /// 性能分析
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseMiniProfilerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var miniprofilerOptions = app.ApplicationServices.GetRequiredService<IOptions<MiniprofilerOptions>>().Value;

        if (!miniprofilerOptions.IsEnabled) return app;

        // 性能分析
        app.UseMiniProfiler();

        return app;
    }
}