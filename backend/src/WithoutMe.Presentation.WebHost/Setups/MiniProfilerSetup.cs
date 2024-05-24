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
    /// 性能分析
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseMiniProfilerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var miniprofilerOptions = app.ApplicationServices.GetRequiredService<IOptions<MiniprofilerOptions>>().Value;

        if (miniprofilerOptions.IsEnabled)
        {
            // 性能分析
            app.UseMiniProfiler();
        }

        return app;
    }
}