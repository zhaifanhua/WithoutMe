#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:GithubOptions
// Guid:bf045211-8a23-473a-a283-bcccc841729c
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 下午 01:54:59
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// GithubOptions
/// </summary>
public class GithubOptions
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
    /// 重定向Url
    /// </summary>
    public string RedirectUrl { get; set; } = string.Empty;

    /// <summary>
    /// 作用域
    /// </summary>
    public string Scope { get; set; } = string.Empty;

    /// <summary>
    /// 授权地址
    /// </summary>
    public string AuthorizeUrl = "https://github.com/login/oauth/authorize";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string AccessTokenUrl = "https://github.com/login/oauth/access_token";

    /// <summary>
    /// 用户信息地址
    /// </summary>
    public string UserInfoUrl = "https://api.github.com/user";
}