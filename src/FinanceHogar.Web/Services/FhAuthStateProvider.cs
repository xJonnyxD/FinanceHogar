using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceHogar.Web.Services;

public class FhAuthStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    private const string TokenKey = "fh_token";
    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(TokenKey);
        if (string.IsNullOrWhiteSpace(token)) return _anonymous;

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwt;
        try { jwt = handler.ReadJwtToken(token); } catch { return _anonymous; }

        if (jwt.ValidTo < DateTime.UtcNow) return _anonymous;

        var claims = jwt.Claims.ToList();
        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var claims = jwt.Claims.ToList();
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout() =>
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
}
