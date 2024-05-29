#region <<版权版本注释>>

// ----------------------------------------------------------------
// Copyright ©2024 ZhaiFanhua All Rights Reserved.
// Licensed under the MulanPSL2 License. See LICENSE in the project root for license information.
// FileName:JwtHandler
// Guid:efb3adbc-934e-4182-b119-0ff6702599d9
// Author:Administrator
// Email:me@zhaifanhua.com
// CreateTime:2024-05-25 上午 10:25:51
// ----------------------------------------------------------------

#endregion <<版权版本注释>>

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using WithoutMe.Domain.Shared.Consts;
using WithoutMe.Presentation.WebHost.Options;
using XiHan.Utils.Extensions.System;

namespace WithoutMe.Presentation.WebHost.Handlers;

/// <summary>
/// Jwt 处理器
/// </summary>
public static class JwtHandler
{
    private static JwtOptions _options = new();

    /// <summary>
    /// 设置 Jwt 配置
    /// </summary>
    /// <param name="options"></param>
    public static void SetJwtOptions([NotNull] JwtOptions options)
    {
        _options = options;
    }

    /// <summary>
    /// 获取 Jwt 配置
    /// </summary>
    /// <returns></returns>
    public static JwtOptions GetJwtOptions()
    {
        try
        {
            // 读取配置
            JwtOptions jwtOptions = _options;
            // 判断结果
            jwtOptions.GetProperties().ForEach(setting =>
            {
                if (setting.PropertyValue.IsNullOrZero()) throw new ArgumentNullException(nameof(setting.PropertyName));
            });
            return jwtOptions;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException($"获取 JwtOptions 配置出错！", ex);
        }
    }

    /// <summary>
    /// Token 颁发
    /// </summary>
    /// <param name="tokenModel"></param>
    /// <returns></returns>
    public static string TokenIssue(TokenModel tokenModel)
    {
        var jwtOptions = GetJwtOptions();

        try
        {
            // 秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwtOptions.SymmetricKey));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha512Signature);

            // Nuget引入：Microsoft.IdentityModel.Tokens
            List<Claim> claims =
            [
                new Claim(ClaimConst.UserId, tokenModel.UserId.ToString()),
                new Claim(ClaimConst.Account, tokenModel.Account),
                new Claim(ClaimConst.NickName, tokenModel.NickName),
                new Claim(ClaimConst.IsSuperAdmin, tokenModel.IsSuperAdmin.ToString())
            ];
            // 用户被分配多个角色
            tokenModel.UserRole.ForEach(role => claims.Add(new Claim(ClaimConst.UserRole, role.ParseToString())));

            // Nuget引入：System.IdentityModel.Tokens.Jwt
            JwtSecurityToken securityToken = new(
                // 自定义选项
                claims: claims,
                // 颁发者
                issuer: jwtOptions.Issuer,
                // 签收者
                audience: jwtOptions.Audience,
                // 秘钥
                signingCredentials: credentials,
                // 生效时间
                notBefore: DateTime.UtcNow,
                // 过期时间
                expires: DateTime.UtcNow.AddMinutes(jwtOptions.Expires)
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return accessToken;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException($"Token 颁发失败！", ex);
        }
    }

    /// <summary>
    /// Token 解析
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static TokenModel TokenSerialize(string token)
    {
        try
        {
            if (!IsSafeVerifyToken(token)) throw new AuthenticationException($"Token 安全验证未通过！");

            token = token.ParseToString().Replace(ClaimConst.TokenReplace, string.Empty);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            List<Claim> claims = jwtToken.Claims.ToList();
            TokenModel tokenModel = new()
            {
                UserId = claims.First(claim => claim.Type == ClaimConst.UserId).Value.ParseToLong(),
                Account = claims.First(claim => claim.Type == ClaimConst.Account).Value,
                NickName = claims.First(claim => claim.Type == ClaimConst.NickName).Value,
                RealName = claims.First(claim => claim.Type == ClaimConst.RealName).Value,
                UserRole = claims.Where(claim => claim.Type == ClaimConst.UserRole).Select(s => s.ParseToLong()).ToList(),
                IsSuperAdmin = claims.First(claim => claim.Type == ClaimConst.IsSuperAdmin).Value.ParseToBool()
            };
            return tokenModel;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException($"Token 解析失败！", ex);
        }
    }

    /// <summary>
    /// Token 安全验证(刷新Token用)
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static bool IsSafeVerifyToken(string token)
    {
        var jwtOptions = GetJwtOptions();

        try
        {
            // 秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwtOptions.SymmetricKey));
            SigningCredentials credentials = new(signingKey, SecurityAlgorithms.HmacSha512Signature);
            token = token.ParseToString().Replace("Bearer ", string.Empty);
            // 开始Token校验
            if (token.IsEmptyOrNull() || !new JwtSecurityTokenHandler().CanReadToken(token))
                throw new ArgumentException("token 为空或无法解析！", nameof(token));
            // 读取旧token
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var verifyResult = jwtToken.RawSignature == JwtTokenUtilities.CreateEncodedSignature(jwtToken.RawHeader + "." + jwtToken.RawPayload, credentials);
            return verifyResult;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException($"Token 被篡改或无效，无法通过安全验证！", ex);
        }
    }

    /// <summary>
    /// 获取 Token 验证参数
    /// </summary>
    /// <returns></returns>
    public static TokenValidationParameters GetTokenVerifyParams()
    {
        var jwtOptions = GetJwtOptions();
        // 签名密钥
        SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(jwtOptions.SymmetricKey));
        // 令牌验证参数
        TokenValidationParameters tokenValidationParameters = new()
        {
            // 是否验证签名
            ValidateIssuerSigningKey = true,
            // 签名
            IssuerSigningKey = signingKey,
            //是否验证颁发者
            ValidateIssuer = true,
            // 颁发者
            ValidIssuer = jwtOptions.Issuer,
            // 是否验证签收者
            ValidateAudience = true,
            // 签收者
            ValidAudience = jwtOptions.Audience,
            // 是否验证失效时间
            ValidateLifetime = true,
            // 过期时间容错值,单位为秒,若为0，过期时间一到立即失效
            ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkew),
            // 需要过期时间
            RequireExpirationTime = true
        };
        return tokenValidationParameters;
    }

    /// <summary>
    /// 读取 Token，不含验证
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public static JsonWebToken ReadJwtToken(string accessToken)
    {
        JsonWebTokenHandler tokenHandler = new();
        return tokenHandler.CanReadToken(accessToken)
            ? tokenHandler.ReadJsonWebToken(accessToken)
            : throw new AuthenticationException("读取 JWT Bearer Token 失败！");
    }

    /// <summary>
    /// 读取 Token，不含验证
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public static JwtSecurityToken SecurityReadJwtToken(string accessToken)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        return jwtSecurityTokenHandler.CanReadToken(accessToken)
            ? jwtSecurityTokenHandler.ReadJwtToken(accessToken)
            : throw new AuthenticationException("读取 JWT Bearer Token 失败！");
    }

    /// <summary>
    /// 获取 JWT Bearer Token
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="headerKey"></param>
    /// <param name="tokenPrefix"></param>
    /// <returns></returns>
    public static string GetJwtBearerToken(HttpContext httpContext, string headerKey = "Authorization", string tokenPrefix = "Bearer ")
    {
        // 判断请求报文头中是否有 "Authorization" 报文头
        var bearerToken = httpContext.Request.Headers[headerKey].ToString();
        if (string.IsNullOrWhiteSpace(bearerToken)) throw new AuthenticationException("获取 JWT Bearer Token 失败！");

        var prefixLenght = tokenPrefix.Length;
        return bearerToken.StartsWith(tokenPrefix, true, null) && bearerToken.Length > prefixLenght
            ? bearerToken[prefixLenght..]
            : throw new AuthenticationException("获取 JWT Bearer Token 失败！");
    }
}

/// <summary>
/// Token 模型
/// </summary>
public class TokenModel
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 用户权限
    /// </summary>
    public List<long> UserRole { get; set; } = [];

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    public bool IsSuperAdmin { get; set; }
}