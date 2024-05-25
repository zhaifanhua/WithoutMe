#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:AlipayOptions
// Guid:865237d4-e995-4c46-a657-022f38e32fee
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 11:20:39
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// AlipayOptions
/// </summary>
public class AlipayOptions
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 重定向Url
    /// </summary>
    public string RedirectUrl { get; set; } = string.Empty;

    /// <summary>
    /// 作用域
    /// </summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// 私钥
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// 公钥
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// 授权地址
    /// </summary>
    public string AuthorizeUrl = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string AccessTokenUrl = "https://openapi.alipay.com/gateway.do";

    /// <summary>
    /// 用户信息地址
    /// </summary>
    public string UserInfoUrl = "https://openapi.alipay.com/gateway.do";
}