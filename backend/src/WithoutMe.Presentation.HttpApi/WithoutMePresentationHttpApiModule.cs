#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMePresentationHttpApiModule
// Guid:dc96fe73-4b39-42a5-a96b-8bf7f83cc882
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-29 上午 11:08:37
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.Modularity;
using WithoutMe.Application.Contracts;

namespace WithoutMe.Presentation.HttpApi;

/// <summary>
/// 无我界面接口模块
/// </summary>
[DependsOn(
    typeof(WithoutMeApplicationContractsModule)
    )]
public class WithoutMePresentationHttpApiModule : AbpModule
{
}