#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:CacheOptions
// Guid:1b55eb4a-9034-4867-9601-f6f35d6ef4b2
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-28 上午 09:59:28
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// CacheOptions
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// 同步时间
    /// </summary>
    public int SyncTimeout { get; set; }

    /// <summary>
    /// 分布式
    /// </summary>
    public class RedisCache
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
    }

    /// <summary>
    /// 响应缓存
    /// </summary>
    public class ResponseCache
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}