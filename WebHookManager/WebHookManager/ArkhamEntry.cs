using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebHookManager;

[Table("arkham_entry")]
public sealed class ArkhamEntry : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("hook_id")]
    public string HookId { get; set; }
    
    [Column("entry")]
    public string Entry { get; set; }
}