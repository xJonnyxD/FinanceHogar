using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using FinanceHogar.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace FinanceHogar.Web.Services;

public class AuthService(HttpClient http, ILocalStorageService localStorage,
    AuthenticationStateProvider authStateProvider)
{
    private const string TokenKey = "fh_token";
    private const string SessionKey = "fh_session";

    public async Task<(bool Ok, string? Error)> LoginAsync(string email, string password)
    {
        try
        {
            var resp = await http.PostAsJsonAsync("api/v1/auth/login", new { email, password });
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                // Extraer mensaje legible del JSON de error si existe
                try
                {
                    var errObj = System.Text.Json.JsonDocument.Parse(err);
                    if (errObj.RootElement.TryGetProperty("message", out var msg))
                        return (false, msg.GetString());
                    if (errObj.RootElement.TryGetProperty("title", out var title))
                        return (false, title.GetString());
                }
                catch { }
                return (false, string.IsNullOrWhiteSpace(err) ? $"Error {(int)resp.StatusCode}" : err);
            }
            var data = await resp.Content.ReadFromJsonAsync<LoginResponse>();
            if (data is null) return (false, "Respuesta inválida");

            await localStorage.SetItemAsync(TokenKey, data.Token);
            var session = new UserSession(data.UsuarioId, data.HogarId, data.NombreCompleto, data.Email, data.Token);
            await localStorage.SetItemAsync(SessionKey, session);

            ((FhAuthStateProvider)authStateProvider).NotifyUserAuthentication(data.Token);
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
            return (true, null);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool Ok, string? Error)> RegisterAsync(RegisterRequest req)
    {
        try
        {
            var resp = await http.PostAsJsonAsync("api/v1/auth/register", req);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync();
                try
                {
                    var errObj = System.Text.Json.JsonDocument.Parse(err);
                    // FluentValidation / ProblemDetails: extraer errores de campos
                    if (errObj.RootElement.TryGetProperty("errors", out var errors))
                    {
                        var msgs = new List<string>();
                        foreach (var field in errors.EnumerateObject())
                            foreach (var msg in field.Value.EnumerateArray())
                                msgs.Add(msg.GetString() ?? "");
                        if (msgs.Count > 0)
                            return (false, string.Join(" | ", msgs));
                    }
                    if (errObj.RootElement.TryGetProperty("message", out var message))
                        return (false, message.GetString());
                    if (errObj.RootElement.TryGetProperty("title", out var title))
                        return (false, title.GetString());
                }
                catch { }
                return (false, string.IsNullOrWhiteSpace(err) ? $"Error {(int)resp.StatusCode}" : err);
            }
            var data = await resp.Content.ReadFromJsonAsync<LoginResponse>();
            if (data is null) return (false, "Respuesta inválida");

            await localStorage.SetItemAsync(TokenKey, data.Token);
            var session = new UserSession(data.UsuarioId, data.HogarId, data.NombreCompleto, data.Email, data.Token);
            await localStorage.SetItemAsync(SessionKey, session);

            ((FhAuthStateProvider)authStateProvider).NotifyUserAuthentication(data.Token);
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.Token);
            return (true, null);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task LogoutAsync()
    {
        await localStorage.RemoveItemAsync(TokenKey);
        await localStorage.RemoveItemAsync(SessionKey);
        ((FhAuthStateProvider)authStateProvider).NotifyUserLogout();
        http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<UserSession?> GetSessionAsync() =>
        await localStorage.GetItemAsync<UserSession>(SessionKey);

    public async Task InitializeAsync()
    {
        var token = await localStorage.GetItemAsync<string>(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
