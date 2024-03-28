using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using SharedNotes.Model;

namespace SharedNotes.Interfaces;

public interface IElasticsearchService
{
    Task<bool> IndexExists(string indexName);
    Task<string> CreateIndexIfNotExists(string indexName);
    Task<DeleteIndexResponse> DeleteIndexIfExists(string indexName);
    Task<IndexResponse> IndexDoc<T>(T doc, string indexName) where T : class;
    Task<BulkResponse> IndexDocs<T>(IEnumerable<T> docs, string indexName) where T : class;
    Task<IEnumerable<Note>> GetNotes(string searchString, string indexName, int from = 0, int size = 0);
}