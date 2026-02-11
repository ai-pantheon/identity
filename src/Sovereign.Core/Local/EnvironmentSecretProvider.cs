namespace Sovereign.Core.Local;

/// <summary>
/// Reads secrets from environment variables. For local development.
/// No cloud dependency. No external service. Just your machine.
/// </summary>
public class EnvironmentSecretProvider : ISecretProvider
{
    public Task<string> GetSecretAsync(string secretName)
    {
        var value = Environment.GetEnvironmentVariable(secretName)
            ?? throw new KeyNotFoundException(
                $"Secret '{secretName}' not found in environment variables.");
        return Task.FromResult(value);
    }
}
