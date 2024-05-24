#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:SwaggerOptions
// Guid:348ad226-038a-480a-9394-14b188fcdb7e
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-23 上午 09:27:47
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Options;

/// <summary>
/// 文档
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// 路由前缀
    /// </summary>
    public string RoutePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 需要暴露的分组
    /// </summary>
    public string[] PublishGroup { get; set; } = [];
}