#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeApplicationModule
// Guid:1b7a33b0-d261-4d23-92bd-73cd787f25c5
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 01:21:02
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.Modularity;
using WithoutMe.Domain.Shared;
using WithoutMe.Infrastructure;
using WithoutMe.Infrastructure.Persistence;

namespace WithoutMe.Application;

/// <summary>
/// 无我应用模块
/// </summary>
[DependsOn(
    typeof(WithoutMeDomainSharedModule),
    typeof(WithoutMeInfrastructureModule),
    typeof(WithoutMeInfrastructurePersistenceModule)
    )]
public class WithoutMeApplicationModule : AbpModule
{
}