#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeDomainModule
// Guid:75f40c89-fbb0-4503-bfb6-2f94448d5701
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 01:21:36
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace WithoutMe.Domain;

/// <summary>
/// 无我领域模块
/// </summary>
[DependsOn(typeof(AbpIdentityDomainModule))]
public class WithoutMeDomainModule : AbpModule
{
}