namespace Sovereign.Core;

/// <summary>
/// Provider-agnostic document store interface.
/// Implementations: Firestore, MongoDB, PostgreSQL, in-memory, etc.
/// Your data layer runs on any cloud — or on your own machine.
/// </summary>
public interface IDocumentStore
{
    /// <summary>Get a typed document by ID. Returns null if not found.</summary>
    Task<T?> GetAsync<T>(string collection, string documentId) where T : class;

    /// <summary>Check if a document exists.</summary>
    Task<bool> ExistsAsync(string collection, string documentId);

    /// <summary>Set (create or overwrite) a document with a specific ID.</summary>
    Task SetAsync<T>(string collection, string documentId, T document) where T : class;

    /// <summary>Add a document with an auto-generated ID. Returns the generated ID.</summary>
    Task<string> AddAsync<T>(string collection, T document) where T : class;

    /// <summary>Partial update — only the specified fields are modified.</summary>
    Task UpdateAsync(string collection, string documentId, Dictionary<string, object> updates);

    /// <summary>Delete a document.</summary>
    Task DeleteAsync(string collection, string documentId);

    /// <summary>Get a document as a raw dictionary. Returns null if not found.</summary>
    Task<Dictionary<string, object>?> GetRawAsync(string collection, string documentId);

    /// <summary>Get a single field value from a document. Returns default if not found.</summary>
    Task<TField?> GetFieldAsync<TField>(string collection, string documentId, string fieldName);

    /// <summary>Start building a query against a collection.</summary>
    IDocumentQuery<T> Query<T>(string collection) where T : class;

    /// <summary>Get all documents in a collection as typed objects.</summary>
    Task<List<T>> GetAllAsync<T>(string collection) where T : class;
}

/// <summary>
/// Fluent query builder for document stores.
/// </summary>
public interface IDocumentQuery<T> where T : class
{
    IDocumentQuery<T> WhereEqualTo(string field, object value);
    IDocumentQuery<T> WhereIn(string field, IEnumerable<object> values);
    IDocumentQuery<T> OrderByDescending(string field);
    IDocumentQuery<T> Limit(int limit);

    Task<List<T>> ExecuteAsync();
    Task<List<Dictionary<string, object>>> ExecuteRawAsync();
    Task<int> CountAsync();
}
