#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:ClassController
// Guid:165dab0e-f255-41fa-b7d7-9d5356a542d7
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-17 下午 03:31:14
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.AspNetCore.Mvc;

namespace WithoutMe.Presentation.WebHost.Controllers;

/// <summary>
/// ClassController
/// </summary>
public class ClassController : ControllerBase
{
    /// <summary>
    /// Index
    /// </summary>
    /// <returns></returns>
    [HttpGet("/api/class")]
    public IActionResult Index()
    {
        return Ok("Hello World!");
    }
}