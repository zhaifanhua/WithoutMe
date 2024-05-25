#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:QQOptions
// Guid:83b0660a-4ba4-4553-abb5-f62e05ffadec
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 下午 01:56:17
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// QQOptions
/// </summary>
public class QQOptions
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// 重定向地址
    /// </summary>
    public string RedirectUrl { get; set; } = string.Empty;

    /// <summary>
    /// 作用域
    /// </summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// 授权地址
    /// </summary>
    public string AuthorizeUrl = "https://graph.qq.com/oauth2.0/authorize";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string AccessTokenUrl = "https://graph.qq.com/oauth2.0/token";

    /// <summary>
    /// OpenId地址
    /// </summary>
    public string OpenIdUrl = "https://graph.qq.com/oauth2.0/me";

    /// <summary>
    /// 用户信息地址
    /// </summary>
    public string UserInfoUrl = "https://graph.qq.com/user/get_user_info";
}