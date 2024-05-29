#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:Program1
// Guid:e769c83f-a83e-4196-9a13-7d50f3ecb2b1
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 04:13:39
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using WithoutMe.Presentation.WebHost;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var host = builder.WebHost;
// 端口
var port = config.GetValue<int>("WebHost:Port");
host.UseUrls($"http://*:{port}");
// 设置接口超时时间和上传大小
host.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
    // 文件上传最大限制 1GB
    options.Limits.MaxRequestBodySize = 1 * 1024 * 1024 * 1024;
});

var services = builder.Services;
services.AddApplication<WithoutMePresentationWebHostModule>();

var app = builder.Build();
app.InitializeApplication();

app.Run();