#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:DataBaseTypeEnum
// Guid:eb1f6b9c-c65d-48fa-8d82-6981271c11c5
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-28 下午 05:11:50
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using System.ComponentModel.DataAnnotations;

namespace WithoutMe.Infrastructure.Persistence.Options;

/// <summary>
/// 数据库类型
/// </summary>
public enum DataBaseTypeEnum
{
    /// <summary>
    /// MySql
    /// </summary>
    [Display(Name = "MySql")] MySql = 0,

    /// <summary>
    /// SqlServer
    /// </summary>
    [Display(Name = "SqlServer")] SqlServer = 1,

    /// <summary>
    /// Sqlite
    /// </summary>
    [Display(Name = "Sqlite")] Sqlite = 2,

    /// <summary>
    /// Oracle
    /// </summary>
    [Display(Name = "Oracle")] Oracle = 3,

    /// <summary>
    /// PostgreSql
    /// </summary>
    [Display(Name = "PostgreSql")] PostgreSql = 4
}