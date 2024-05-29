#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:RedisCacheOptions
// Guid:7ee7cd83-a513-439a-a297-ecdde8e4145b
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2024/5/30 3:15:53
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Infrastructure.Caching.Options;

/// <summary>
/// RedisCacheOptions
/// </summary>
public class RedisCacheOptions
{
    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 前辍
    /// </summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// 同步时间
    /// </summary>
    public int SyncTimeout { get; set; }
}