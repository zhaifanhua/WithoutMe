#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:DatabaseOptions
// Guid:5e0b073f-022c-41d8-8ad3-b4b35769af55
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 06:06:14
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// DatabaseOptions
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public DatabaseConfig[] DatabaseConfigs { get; set; } = [];

    /// <summary>
    /// 控制台打印
    /// </summary>
    public bool Console { get; set; }

    /// <summary>
    /// 日志打印
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        public bool Info { get; set; }

        /// <summary>
        /// 错误日志
        /// </summary>
        public bool Error { get; set; }
    }

    /// <summary>
    /// 是否初始化数据库
    /// </summary>
    public bool EnableInitDb { get; set; }

    /// <summary>
    /// 是否初始化种子数据
    /// </summary>
    public bool EnableInitSeed { get; set; }
}