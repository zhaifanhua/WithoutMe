#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:AppOptions
// Guid:8fef97a6-178b-479e-b2c9-fcfa8371b0ad
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 10:35:14
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// AppOptions
/// </summary>
public partial class AppOptions
{
    /// <summary>
    /// WebHostOptions
    /// </summary>
    public WebHostOptions WebHostOptions { get; set; } = new WebHostOptions();

    /// <summary>
    /// CorsOptions
    /// </summary>
    public CorsOptions CorsOptions { get; set; } = new CorsOptions();

    /// <summary>
    /// SwaggerOptions
    /// </summary>
    public SwaggerOptions SwaggerOptions { get; set; } = new SwaggerOptions();

    /// <summary>
    /// MiniprofilerOptions
    /// </summary>
    public MiniprofilerOptions MiniprofilerOptions { get; set; } = new MiniprofilerOptions();

    /// <summary>
    /// JwtOptions
    /// </summary>
    public JwtOptions JwtOptions { get; set; } = new JwtOptions();

    /// <summary>
    /// AuthOptions
    /// </summary>
    public AuthOptions AuthOptions { get; set; } = new AuthOptions();
}