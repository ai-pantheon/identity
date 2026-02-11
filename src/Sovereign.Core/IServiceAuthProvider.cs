namespace Sovereign.Core;

/// <summary>
/// Provider-agnostic service-to-service authentication.
/// Implementations: GCP metadata server, AWS IAM, Azure Managed Identity, API key.
/// </summary>
public interface IServiceAuthProvider
{
    /// <summary>Get an access token for authenticating to another service.</summary>
    Task<string> GetAccessTokenAsync(string audience);
}
