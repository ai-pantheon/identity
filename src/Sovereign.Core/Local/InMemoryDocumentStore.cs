using System.Collections.Concurrent;
using System.Text.Json;

namespace Sovereign.Core.Local;

/// <summary>
/// In-memory document store for local development and testing.
/// No cloud dependency. Runs anywhere.
/// </summary>
public class InMemoryDocumentStore : IDocumentStore
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _collections = new();
    private int _autoIdCounter = 0;

    private ConcurrentDictionary<string, string> GetCollection(string collection) =>
        _collections.GetOrAdd(collection, _ => new ConcurrentDictionary<string, string>());

    public Task<T?> GetAsync<T>(string collection, string documentId) where T : class
    {
        var col = GetCollection(collection);
        if (col.TryGetValue(documentId, out var json))
            return Task.FromResult(JsonSerializer.Deserialize<T>(json));
        return Task.FromResult<T?>(null);
    }

    public Task<bool> ExistsAsync(string collection, string documentId)
    {
        return Task.FromResult(GetCollection(collection).ContainsKey(documentId));
    }

    public Task SetAsync<T>(string collection, string documentId, T document) where T : class
    {
        GetCollection(collection)[documentId] = JsonSerializer.Serialize(document);
        return Task.CompletedTask;
    }

    public Task<string> AddAsync<T>(string collection, T document) where T : class
    {
        var id = $"auto-{Interlocked.Increment(ref _autoIdCounter)}";
        GetCollection(collection)[id] = JsonSerializer.Serialize(document);
        return Task.FromResult(id);
    }

    public Task UpdateAsync(string collection, string documentId, Dictionary<string, object> updates)
    {
        var col = GetCollection(collection);
        if (col.TryGetValue(documentId, out var json))
        {
            var doc = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new();
            foreach (var kvp in updates)
                doc[kvp.Key] = kvp.Value;
            col[documentId] = JsonSerializer.Serialize(doc);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string collection, string documentId)
    {
        GetCollection(collection).TryRemove(documentId, out _);
        return Task.CompletedTask;
    }

    public Task<Dictionary<string, object>?> GetRawAsync(string collection, string documentId)
    {
        var col = GetCollection(collection);
        if (col.TryGetValue(documentId, out var json))
            return Task.FromResult(JsonSerializer.Deserialize<Dictionary<string, object>>(json));
        return Task.FromResult<Dictionary<string, object>?>(null);
    }

    public Task<TField?> GetFieldAsync<TField>(string collection, string documentId, string fieldName)
    {
        var col = GetCollection(collection);
        if (col.TryGetValue(documentId, out var json))
        {
            var doc = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (doc != null && doc.TryGetValue(fieldName, out var element))
                return Task.FromResult(JsonSerializer.Deserialize<TField>(element.GetRawText()));
        }
        return Task.FromResult<TField?>(default);
    }

    public IDocumentQuery<T> Query<T>(string collection) where T : class =>
        new InMemoryDocumentQuery<T>(GetCollection(collection));

    public Task<List<T>> GetAllAsync<T>(string collection) where T : class
    {
        var col = GetCollection(collection);
        var results = col.Values
            .Select(json => JsonSerializer.Deserialize<T>(json)!)
            .ToList();
        return Task.FromResult(results);
    }
}

public class InMemoryDocumentQuery<T> : IDocumentQuery<T> where T : class
{
    private readonly ConcurrentDictionary<string, string> _collection;
    private int _limit = int.MaxValue;

    public InMemoryDocumentQuery(ConcurrentDictionary<string, string> collection)
    {
        _collection = collection;
    }

    public IDocumentQuery<T> WhereEqualTo(string field, object value) => this;
    public IDocumentQuery<T> WhereIn(string field, IEnumerable<object> values) => this;
    public IDocumentQuery<T> OrderByDescending(string field) => this;
    public IDocumentQuery<T> Limit(int limit) { _limit = limit; return this; }

    public Task<List<T>> ExecuteAsync()
    {
        var results = _collection.Values
            .Take(_limit)
            .Select(json => JsonSerializer.Deserialize<T>(json)!)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<List<Dictionary<string, object>>> ExecuteRawAsync()
    {
        var results = _collection.Values
            .Take(_limit)
            .Select(json => JsonSerializer.Deserialize<Dictionary<string, object>>(json)!)
            .ToList();
        return Task.FromResult(results);
    }

    public Task<int> CountAsync() =>
        Task.FromResult(Math.Min(_collection.Count, _limit));
}
