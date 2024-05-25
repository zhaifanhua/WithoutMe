#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:MapsterSetup
// Guid:d8fae7ff-7cf8-4957-bf3f-0db3fd3378e7
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 09:40:40
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using MapsterMapper;
using WithoutMe.Presentation.WebHost.Common.Mapster;

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// MapsterSetup
/// </summary>
public static class MapsterSetup
{
    /// <summary>
    /// Mapster 服务扩展
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddMapsterSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 创建具体的映射对象
        services.AddSingleton(MapsterAdaptConfig.InitMapperConfig());
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}