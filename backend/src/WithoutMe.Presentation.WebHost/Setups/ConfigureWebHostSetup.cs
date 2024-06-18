#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ConfigureWebHostSetup
// Guid:71a80fad-b529-4aed-a7c9-73b106302c04
// Author:zhaifanhua
// Email:me@zhaifanhua.com
// CreateTime:2024/6/18 13:08:24
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

namespace WithoutMe.Presentation.WebHost.Setups;

/// <summary>
/// ConfigureWebHostSetup
/// </summary>
public static class ConfigureWebHostSetup
{
    /// <summary>
    /// Host主机扩展
    /// </summary>
    /// <param name="host"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static ConfigureWebHostBuilder AddConfigureWebHostSetup(this ConfigureWebHostBuilder host, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(host);

        // 端口
        var port = config.GetValue<int>("Port");
        host.UseUrls($"http://*:{port}");

        // 设置接口超时时间和上传大小
        host.ConfigureKestrel(options =>
        {
            options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
            // 文件上传最大限制 1GB
            options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024;
        });

        return host;
    }
}