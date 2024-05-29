#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WithoutMeInfrastructurePersistenceModule
// Guid:6848d1a3-fc52-45a6-b0cc-48b6a78d69c9
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 02:52:48
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;
using WithoutMe.Domain;

namespace WithoutMe.Infrastructure.Persistence;

/// <summary>
/// 无我基础设施数据持久化模块
/// </summary>
[DependsOn(
    typeof(WithoutMeDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class WithoutMeInfrastructurePersistenceModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}