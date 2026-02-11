namespace Sovereign.Core;

/// <summary>
/// Provider-agnostic secret retrieval.
/// Implementations: GCP Secret Manager, AWS Secrets Manager, Azure Key Vault, environment variables.
/// </summary>
public interface ISecretProvider
{
    /// <summary>Retrieve a secret by name.</summary>
    Task<string> GetSecretAsync(string secretName);
}
