#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:GiteeOptions
// Guid:26219c2f-af2b-4e5b-89da-2ae8d1750f22
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 下午 01:54:23
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// GiteeOptions
/// </summary>
public class GiteeOptions
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
    public string AuthorizeUrl = "https://gitee.com/oauth/authorize";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string AccessTokenUrl = "https://gitee.com/oauth/token";

    /// <summary>
    /// 用户信息地址
    /// </summary>
    public string UserInfoUrl = "https://gitee.com/api/v5/user";
}