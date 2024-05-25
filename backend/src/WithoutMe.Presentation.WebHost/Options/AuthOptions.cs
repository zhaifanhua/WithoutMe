#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:AuthOptions
// Guid:06f941f1-706d-41af-8aec-7696577dbbe0
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-24 下午 06:05:10
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using WithoutMe.Presentation.WebHost.Options.Authorize;

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// 授权
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Github
    /// </summary>
    public GithubOptions Github { get; set; } = new GithubOptions();

    /// <summary>
    /// Gitee
    /// </summary>
    public GiteeOptions Gitee { get; set; } = new GiteeOptions();

    /// <summary>
    /// Alipay
    /// </summary>
    public AlipayOptions Alipay { get; set; } = new AlipayOptions();

    /// <summary>
    /// Dingtalk
    /// </summary>
    public DingtalkOptions Dingtalk { get; set; } = new DingtalkOptions();

    /// <summary>
    /// Microsoft
    /// </summary>
    public MicrosoftOptions Microsoft { get; set; } = new MicrosoftOptions();

    /// <summary>
    /// Weibo
    /// </summary>
    public WeiboOptions Weibo { get; set; } = new WeiboOptions();

    /// <summary>
    /// QQ
    /// </summary>
    public QQOptions QQ { get; set; } = new QQOptions();
}