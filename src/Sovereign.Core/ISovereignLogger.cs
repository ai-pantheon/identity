namespace Sovereign.Core;

/// <summary>
/// Provider-agnostic structured logging.
/// Implementations: GCP Cloud Logging, AWS CloudWatch, Azure Monitor, console.
/// </summary>
public interface ISovereignLogger
{
    void Info(string message, Dictionary<string, object>? labels = null);
    void Warning(string message, Dictionary<string, object>? labels = null);
    void Error(string message, Exception? exception = null, Dictionary<string, object>? labels = null);
}
