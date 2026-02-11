namespace Sovereign.Core.Local;

/// <summary>
/// Simple API key authentication for local development.
/// No cloud dependency.
/// </summary>
public class ApiKeyAuthProvider : IServiceAuthProvider
{
    private readonly string _apiKey;

    public ApiKeyAuthProvider(string apiKey) => _apiKey = apiKey;

    public Task<string> GetAccessTokenAsync(string audience) =>
        Task.FromResult(_apiKey);
}
