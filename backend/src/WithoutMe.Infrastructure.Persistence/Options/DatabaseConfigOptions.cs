#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:DatabaseConfigOptions
// Guid:bf9a2a4b-f2a2-4a8f-b627-d60090c2cf3d
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-28 下午 05:13:02
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Infrastructure.Persistence.Options;

/// <summary>
/// 数据库配置
/// </summary>
public class DatabaseConfigOptions
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public DatabaseOptions[] DatabaseOptions { get; set; } = [];

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