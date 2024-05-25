#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:WriteResponseHandler
// Guid:42efec58-361d-4b87-9776-677c26a09237
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 10:59:53
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.Extensions.Diagnostics.HealthChecks;
using XiHan.Utils.System.Text.Json.Serialization;

namespace WithoutMe.Presentation.WebHost.Handlers;

/// <summary>
/// WriteResponseHandler
/// </summary>
public static class WriteResponseHandler
{
    /// <summary>
    /// 重写响应
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="healthReport"></param>
    /// <returns></returns>
    public static async Task WriteResponse(HttpContext httpContext, HealthReport healthReport)
    {
        var result = new HealthCheckResult
        {
            Status = healthReport.Status.ToString(),
            Results = healthReport.Entries.ToDictionary(entry => entry.Key, entry => new HealthCheckEntry
            {
                Status = entry.Value.Status.ToString(),
                Description = entry.Value.Description ?? string.Empty,
                Data = entry.Value.Data
            })
        };

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(SerializeHelper.SerializeTo(result));
    }
}

/// <summary>
/// 健康检查结果
/// </summary>
public class HealthCheckResult
{
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 结果
    /// </summary>
    public IDictionary<string, HealthCheckEntry> Results { get; set; } = new Dictionary<string, HealthCheckEntry>();
}

/// <summary>
/// 健康检查项
/// </summary>
public class HealthCheckEntry
{
    /// <summary>
    /// 状态
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 数据
    /// </summary>
    public object Data { get; set; } = new object();
}