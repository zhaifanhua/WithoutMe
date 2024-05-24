#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:CorsOptions
// Guid:1bd6bc2a-799b-464a-af9e-ba04725ce39e
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 05:07:02
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// 跨域
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// 是否可用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 策略名称
    /// </summary>
    public string PolicyName { get; set; } = string.Empty;

    /// <summary>
    /// 域名
    /// </summary>
    public string[] Origins { get; set; } = [];

    /// <summary>
    /// 请求头
    /// </summary>
    public string[] Headers { get; set; } = [];
}