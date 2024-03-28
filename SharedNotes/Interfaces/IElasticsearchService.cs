using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using SharedNotes.Model;

namespace SharedNotes.Interfaces;

public interface IElasticsearchService
{
    Task<bool> IndexExistsAsync(string indexName);
    Task<string> CreateIndexIfNotExistsAsync(string indexName);
    Task<DeleteIndexResponse> DeleteIndexIfExistsAsync(string indexName);
    Task<IndexResponse> IndexDocAsync<T>(T doc, string indexName) where T : class;
    Task<BulkResponse> IndexDocsAsync<T>(IEnumerable<T> docs, string indexName) where T : class;
    Task<IEnumerable<Note>> GetNotesAsync(string searchString, string indexName, int from = 0, int size = 0);
    Task<List<T>?> GetAll<T>(string indexName) where T : class;
}