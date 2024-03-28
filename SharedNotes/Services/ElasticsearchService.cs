using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using SharedNotes.Interfaces;
using SharedNotes.Model;
using ExistsResponse = Elastic.Clients.Elasticsearch.ExistsResponse;

namespace SharedNotes.Services;

public class ElasticsearchService : IElasticsearchService
{
    private ElasticsearchClient _client;

    public ElasticsearchService(ElasticsearchClient client)
    {
        _client = client;
    }
    
    public async Task<bool> IndexExistsAsync(string indexName)
    {
        var result = await _client.Indices.ExistsAsync(indexName);

        if (result.IsValidResponse)
        {
            return result.Exists;   
        }

        return false;
    }

    public async Task<string> CreateIndexIfNotExistsAsync(string indexName)
    {
        if (await IndexExistsAsync(indexName))
        {
            return indexName;
        }
        
        var newIndex =  await _client.Indices.CreateAsync(indexName);

        if (newIndex.IsValidResponse)
        {
            return newIndex.Index;
        }
        
        return null!;
    }

    public async Task<DeleteIndexResponse> DeleteIndexIfExistsAsync(string indexName)
    {
        if (await IndexExistsAsync(indexName))
        {
            return await _client.Indices.DeleteAsync(indexName);
        }

        return null!;
    }
    
    public async Task<IndexResponse> IndexDocAsync<T>(T doc, string indexName) where T : class
    {
        return await _client.IndexAsync(doc, indexName);
    }

    public async Task<BulkResponse> IndexDocsAsync<T>(IEnumerable<T> docs, string indexName) where T : class
    {
        return await _client.IndexManyAsync(docs, indexName);
    }
    
    public async Task<IEnumerable<Note>> GetNotesAsync(string searchString, string indexName, int from = 0, int size = 0)
    {
        var foundNotes = new List<Note>();
        
        var response = await _client.SearchAsync<Note>(s => s
            .Index(indexName)
            .From(from)
            .Size(size)
            .Query(q => q
                .Term(t => t.Title, searchString)
            )
        );

        if (response.IsValidResponse)
        {
            foundNotes.AddRange(response.Documents.AsEnumerable());
        }
        
        response = await _client.SearchAsync<Note>(s => s
            .Index(indexName)
            .From(from)
            .Size(size)
            .Query(q => q
                .Term(t => t.Body, searchString)
            )
        );

        if (response.IsValidResponse)
        {
            foundNotes.AddRange(response.Documents.AsEnumerable());
        }

        return foundNotes;
    }
    
    public async Task<List<T>?> GetAll<T>(string indexName) where T : class
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(indexName).Query(q => q.MatchAll()));
        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }
}