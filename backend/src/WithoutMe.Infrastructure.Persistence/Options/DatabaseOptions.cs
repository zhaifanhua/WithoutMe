#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:DatabaseOptions
// Guid:b3b623c1-ab3b-414d-900d-cd5c7346f460
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-28 下午 05:11:06
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Infrastructure.Persistence.Options;

/// <summary>
/// 数据库
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// 连接Id
    /// </summary>
    public int ConfigId { get; set; }

    /// <summary>
    /// 数据库类型
    /// DataBaseTypeEnum
    /// </summary>
    public string DataBaseType { get; set; } = string.Empty;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 是否自动关闭连接
    /// </summary>
    public bool IsAutoCloseConnection { get; set; }
}