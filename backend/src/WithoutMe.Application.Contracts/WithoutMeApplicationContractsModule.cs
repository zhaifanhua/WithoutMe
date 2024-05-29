#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeApplicationContractsModule
// Guid:113f9b23-53ee-4891-a507-12fe0cbf64d2
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-29 上午 10:32:58
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.Application;
using Volo.Abp.Modularity;
using WithoutMe.Domain.Shared;

namespace WithoutMe.Application.Contracts;

/// <summary>
/// 无我应用契约模块
/// </summary>
[DependsOn(
    typeof(WithoutMeDomainSharedModule),
    typeof(AbpDddApplicationContractsModule)
    )]
public class WithoutMeApplicationContractsModule : AbpModule
{
}