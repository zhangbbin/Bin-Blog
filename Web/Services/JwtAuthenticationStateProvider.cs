using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Bin_Blog.Web.Services;

/// <summary>
/// 自定义 AuthenticationStateProvider：从 localStorage 中读取 JWT，
/// 解析 Claims 后提供给 Blazor 的授权系统（[Authorize]、AuthorizeView 等）。
/// </summary>
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;
    private static readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public JwtAuthenticationStateProvider(IJSRuntime js)
    {
        _js = js;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var claims = ParseClaimsFromJwt(token);
            if (claims == null)
            {
                return new AuthenticationState(_anonymous);
            }

            // 检查 token 是否已过期
            var expClaim = claims.FindFirst(JwtRegisteredClaimNames.Exp)
                        ?? claims.FindFirst("exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out var exp))
            {
                var expTime = DateTimeOffset.FromUnixTimeSeconds(exp);
                if (expTime <= DateTimeOffset.UtcNow)
                {
                    return new AuthenticationState(_anonymous);
                }
            }

            return new AuthenticationState(new ClaimsPrincipal(claims));
        }
        catch
        {
            // 预渲染阶段 JS 不可用，返回匿名
            return new AuthenticationState(_anonymous);
        }
    }

    /// <summary>
    /// 登录成功后通知 Blazor 授权系统状态变更。
    /// </summary>
    public void NotifyUserAuthentication(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        var user = claims != null ? new ClaimsPrincipal(claims) : _anonymous;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    /// <summary>
    /// 退出登录后通知 Blazor 授权系统状态变更。
    /// </summary>
    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    private static ClaimsIdentity? ParseClaimsFromJwt(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // 将 JWT Claims 映射为标准 ClaimsIdentity
            var claims = new List<Claim>();
            foreach (var claim in jwt.Claims)
            {
                // JWT 的 "role" claim 需要映射到 ClaimTypes.Role 才能让 [Authorize(Roles=...)] 生效
                if (claim.Type == "role" || claim.Type == ClaimTypes.Role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                }
                else if (claim.Type == JwtRegisteredClaimNames.Sub || claim.Type == ClaimTypes.NameIdentifier)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                }
                else if (claim.Type == JwtRegisteredClaimNames.UniqueName || claim.Type == ClaimTypes.Name)
                {
                    claims.Add(new Claim(ClaimTypes.Name, claim.Value));
                }
                else
                {
                    claims.Add(claim);
                }
            }

            return new ClaimsIdentity(claims, "jwt");
        }
        catch
        {
            return null;
        }
    }
}
