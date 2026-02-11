using Sovereign.Core.Local;
using Microsoft.Extensions.DependencyInjection;

namespace Sovereign.Core;

/// <summary>
/// Central DI registration for all Sovereign infrastructure services.
/// Reads SOVEREIGN_PROVIDER env var to select implementations.
/// Default: "local". Alternatives: "gcp", "aws", "azure".
/// </summary>
public static class SovereignServiceRegistration
{
    public static IServiceCollection AddSovereignInfrastructure(
        this IServiceCollection services,
        string serviceName)
    {
        var provider = Environment.GetEnvironmentVariable("SOVEREIGN_PROVIDER") ?? "local";

        switch (provider.ToLowerInvariant())
        {
            case "local":
            default:
                services.AddSingleton<IDocumentStore>(_ => new InMemoryDocumentStore());
                services.AddSingleton<ISecretProvider>(_ => new EnvironmentSecretProvider());
                services.AddSingleton<IServiceAuthProvider>(_ =>
                    new ApiKeyAuthProvider(
                        Environment.GetEnvironmentVariable("SOVEREIGN_API_KEY") ?? "dev-key"));
                services.AddSingleton<ISovereignLogger>(_ => new ConsoleLogger(serviceName));
                break;

            // Cloud provider implementations can be registered by installing
            // the corresponding NuGet package:
            //   Sovereign.Gcp, Sovereign.Aws, Sovereign.Azure
            //
            // case "gcp":
            //     services.AddSingleton<IDocumentStore>(_ =>
            //         new FirestoreDocumentStore(projectId, databaseId));
            //     break;
        }

        return services;
    }
}
