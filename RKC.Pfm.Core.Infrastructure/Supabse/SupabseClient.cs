using Postgrest.Interfaces;
using Supabase;
using Supabase.Functions.Interfaces;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Realtime.Interfaces;
using Supabase.Storage;
using Supabase.Storage.Interfaces;
using Client = Supabase.Client;

namespace RKC.Pfm.Core.Infrastructure.Supabse;

public interface ISupabseClient: ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject>
{
    
}

public class SupabseClient: Client, ISupabseClient
{
    public SupabseClient(IGotrueClient<User, Session> auth, IRealtimeClient<RealtimeSocket, RealtimeChannel> realtime, IFunctionsClient functions, IPostgrestClient postgrest, IStorageClient<Bucket, FileObject> storage, SupabaseOptions options) : base(auth, realtime, functions, postgrest, storage, options)
    {
    }

    public SupabseClient(string supabaseUrl, string? supabaseKey, SupabaseOptions? options = null) : base(supabaseUrl, supabaseKey, options)
    {
    }
}