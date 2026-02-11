namespace Sovereign.Core.Local;

/// <summary>
/// Console-based structured logger for local development.
/// No cloud dependency. Writes to stdout.
/// </summary>
public class ConsoleLogger : ISovereignLogger
{
    private readonly string _serviceName;

    public ConsoleLogger(string serviceName) => _serviceName = serviceName;

    public void Info(string message, Dictionary<string, object>? labels = null) =>
        Log("INFO", message, labels);

    public void Warning(string message, Dictionary<string, object>? labels = null) =>
        Log("WARN", message, labels);

    public void Error(string message, Exception? exception = null, Dictionary<string, object>? labels = null) =>
        Log("ERROR", exception != null ? $"{message} — {exception.Message}" : message, labels);

    private void Log(string level, string message, Dictionary<string, object>? labels)
    {
        var timestamp = DateTime.UtcNow.ToString("HH:mm:ss");
        var labelStr = labels != null
            ? " " + string.Join(", ", labels.Select(l => $"{l.Key}={l.Value}"))
            : "";
        Console.WriteLine($"[{timestamp}] [{level}] [{_serviceName}] {message}{labelStr}");
    }
}
