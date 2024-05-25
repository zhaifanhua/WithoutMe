#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WeiboOptions
// Guid:1de94517-2d1a-4014-8f59-f869450632a9
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 下午 01:56:56
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// WeiboOptions
/// </summary>
public class WeiboOptions
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
    public string AuthorizeUrl = "https://api.weibo.com/oauth2/authorize";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string AccessTokenUrl = "https://api.weibo.com/oauth2/access_token";

    /// <summary>
    /// 用户信息地址
    /// </summary>
    public string UserInfoUrl = "https://api.weibo.com/2/users/show.json";
}