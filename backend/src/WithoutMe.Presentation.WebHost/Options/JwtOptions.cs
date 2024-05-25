#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:JwtOptions
// Guid:147edd6c-998e-4510-a4b1-905b1e312365
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 09:18:05
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// JwtOptions
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// 颁发者
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// 签收者
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// 秘钥
    /// </summary>
    public string SymmetricKey { get; set; } = string.Empty;

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expires { get; set; }

    /// <summary>
    /// 过期时间容错值
    /// </summary>
    public int ClockSkew { get; set; }
}