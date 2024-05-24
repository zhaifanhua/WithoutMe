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

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// 授权
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Jwt
    /// </summary>
    public class Jwt
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

    /// <summary>
    /// QQ 授权配置
    /// </summary>
    public class Qq
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// WeChat 授权配置
    /// </summary>
    public class WeChat
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// Alipay 授权配置
    /// </summary>
    public class Alipay
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// Github 授权配置
    /// </summary>
    public class Github
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// Gitlab 授权配置
    /// </summary>
    public class Gitlab
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }

    /// <summary>
    /// Gitee 授权配置
    /// </summary>
    public class Gitee
    {
        /// <summary>
        /// 客户端(AppId)
        /// </summary>
        public string ClientId { get; set; } = string.Empty;

        /// <summary>
        /// 客户端密钥(AppKey)
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}