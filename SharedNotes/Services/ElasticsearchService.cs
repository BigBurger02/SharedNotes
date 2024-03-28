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
    
    public async Task<bool> IndexExists(string indexName)
    {
        var result = await _client.Indices.ExistsAsync(indexName);

        if (result.IsValidResponse)
        {
            return result.Exists;   
        }

        return false;
    }

    public async Task<string> CreateIndexIfNotExists(string indexName)
    {
        if (await IndexExists(indexName))
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

    public async Task<DeleteIndexResponse> DeleteIndexIfExists(string indexName)
    {
        if (await IndexExists(indexName))
        {
            return await _client.Indices.DeleteAsync(indexName);
        }

        return null!;
    }
    
    public async Task<IndexResponse> IndexDoc<T>(T doc, string indexName) where T : class
    {
        return await _client.IndexAsync(doc, indexName);
    }

    public async Task<BulkResponse> IndexDocs<T>(IEnumerable<T> docs, string indexName) where T : class
    {
        return await _client.IndexManyAsync(docs, indexName);
    }
    
    public async Task<IEnumerable<Note>> GetNotes(string searchString, string indexName, int from = 0, int size = 0)
    {
        var response = await _client.SearchAsync<Note>(s => s
            .Index(indexName)
            .From(from)
            .Size(size)
            .Query(q => q
                .Term(t => t.Body, searchString)
            )
        );

        if (response.IsValidResponse)
        {
            return response.Documents.AsEnumerable();
        }

        return null!;
    }
}