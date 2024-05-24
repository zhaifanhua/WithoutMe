#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeDomainSharedModule
// Guid:3404fe4f-4512-4497-9506-9bee20c72b7b
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 02:57:54
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace WithoutMe.Domain.Shared;

/// <summary>
/// 无我领域共享模块
/// </summary>
[DependsOn(
    typeof(WithoutMeDomainModule),
    typeof(AbpDddDomainSharedModule)
    )]
public class WithoutMeDomainSharedModule : AbpModule
{
}