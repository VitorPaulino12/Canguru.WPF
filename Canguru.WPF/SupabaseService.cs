using Supabase;
using System.Threading.Tasks;

public static class SupabaseService
{
    private static Supabase.Client _client;

    public static Supabase.Client Client
    {
        get
        {
            if (_client == null)
            {
                var url = "https://lqvxqnvzdjtrssaciryw.supabase.co";
                var key = "sb_publishable_-DyBp_iqj6MoUZc6LYPyMQ_G1IPwEcr";

                _client = new Supabase.Client(url, key, new Supabase.SupabaseOptions
                {
                    AutoConnectRealtime = false
                });
            }

            return _client;
        }
    }

    public static async Task ResetarSenha(string email)
    {
        await Client.Auth.ResetPasswordForEmail(email);
    }
}
