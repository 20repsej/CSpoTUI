using System;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
/*
static async void Main(string[] args)
{
  AuthorizationCodeAuth auth = new AuthorizationCodeAuth(
            "5dc276b9432a4b55b0e1070fa5569441",
            "4b3ee52fd2ef44d4a6ae7a51520d8170",
            "https://mysite.com/callback/",
            "https://mysite.com/callback/",
            Scope.PlaylistReadPrivate | Scope.UserReadCurrentlyPlaying
            );

            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop();
                Token token = await auth.ExchangeCode(payload.Code);
                SetToken(token.AccessToken);

                SpotifyWebAPI api = new SpotifyWebAPI()
                {
                    TokenType = token.TokenType,
                    AccessToken = token.AccessToken,

                };
                // Do requests with API client
            };
            auth.Start(); // Starts an internal HTTP Server
            auth.OpenBrowser();
}
public SpotifyAPI(string clientId, string secretId, string redirectUrl = "http://localhost:4002")
{
    _clientId = clientId;
    _secretId = secretId;

    if (!string.IsNullOrEmpty(_clientId) && !string.IsNullOrEmpty(_secretId))
    {
        var auth = new AuthorizationCodeAuth(_clientId, _secretId, redirectUrl, redirectUrl,
            Scope.Streaming | Scope.PlaylistReadCollaborative | Scope.UserReadCurrentlyPlaying | Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState);
        auth.AuthReceived += AuthOnAuthReceived;
        auth.Start();
        auth.OpenBrowser();
    }
}



*/