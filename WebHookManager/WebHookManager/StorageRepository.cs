using System.Data;

namespace WebHookManager;

using Supabase;

internal interface IStorageRepository
{
    
}

internal sealed class StorageRepository : IStorageRepository
{
    private readonly Client _client;
    
    public StorageRepository(Client client)
    {
        _client = client;
    }
    
    public async Task InsertAsync(string hookId, string message)
    {
        var data = new ArkhamEntry()
        {
            HookId = hookId,
            Entry = message,
            CreatedAt = DateTime.UtcNow
        };
        
        await _client
            .From<ArkhamEntry>()
            .Insert(data);
    }
}