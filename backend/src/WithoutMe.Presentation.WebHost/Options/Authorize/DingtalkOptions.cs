#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:DingtalkOptions
// Guid:fc43d5ca-b2e5-4b3b-b63b-79f6528308aa
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 下午 01:53:35
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options.Authorize;

/// <summary>
/// DingtalkOptions
/// </summary>
public class DingtalkOptions
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public string AppId { get; set; } = string.Empty;

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public string AppSecret { get; set; } = string.Empty;

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
    public string AuthorizeUrl = "https://oapi.dingtalk.com/connect/qrconnect";

    /// <summary>
    /// 访问令牌地址
    /// </summary>
    public string UserInfoUrl = "https://oapi.dingtalk.com/sns/getuserinfo_bycode";
}