/*using BarRaider.SdTools; Inte min kod
using dotenv.net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StreamDeck_Tools_Template1
{
    [PluginActionId("com.bini.spotify")]
    public class PluginAction : PluginBase
    {
        private string clientId;
        private string clientSecret;
        private AuthorizationCodeAuth auth;
        private SpotifyWebAPI api;
        String songId = String.Empty;
        Boolean browserOpen = false;

        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.accessToken = String.Empty;
                return instance;
            }

            [JsonProperty(PropertyName = "accessToken")]
            public string accessToken { get; set; }


            [JsonProperty(PropertyName = "tokenType")]
            public string tokenType { get; set; }

            [JsonProperty(PropertyName = "refreshToken")]
            public string refreshToken { get; set; }

        }

        #region Private Members

        private PluginSettings settings;

        #endregion
        public PluginAction(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            DotEnv.Config();
            clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = PluginSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<PluginSettings>();
            }
        }


        private async void AuthOnAuthReceived(object sender, AuthorizationCode payload)
        {
            var auth = (AuthorizationCodeAuth)sender;
            auth.Stop();
            Token token = await auth.ExchangeCode(payload.Code);
            browserOpen = false;
            api = await initApi(token);
        }

        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override void KeyPressed(KeyPayload payload)
        {
            if (api != null)
            {
                PlaybackContext playback = api.GetPlayback();
                ErrorResponse response;
                if (playback.IsPlaying)
                {
                    response = api.PausePlayback();
                } else
                {
                    response = api.ResumePlayback("", "", null, "");
                }
                if (response.HasError())
                {
                    if (response.Error.Status == 401)
                    {
                        api = null;
                        settings.accessToken = null;
                        SaveSettings();
                    }
                }
                
                Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");
            }
        }

        public override void KeyReleased(KeyPayload payload) { }

        private static Random random = new Random();

        private async Task<SpotifyWebAPI> initApi(Token token)
        {
            var spotifyWebAPI = new SpotifyWebAPI()
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType,
            };
            settings.accessToken = token.AccessToken;
            settings.tokenType = token.TokenType;
            if (token.RefreshToken != null && token.RefreshToken != String.Empty)
            {
                settings.refreshToken = token.RefreshToken;
            }
            await SaveSettings();
            return spotifyWebAPI;
        }

        public async override void OnTick() {
            if (api != null)
            {
                var playback = api.GetPlayback();
                if (playback.HasError()) {
                    bool wasAbleToRefresh = false;
                    if (settings.refreshToken != String.Empty)
                    {
                        this.auth = new AuthorizationCodeAuth(clientId, clientSecret, "http://localhost:4002", "http://localhost:4002",
                                Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative 
                                | Scope.UserReadCurrentlyPlaying | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
                        this.auth.AuthReceived += AuthOnAuthReceived;
                        var token = await auth.RefreshToken(settings.refreshToken);
                        if (!token.HasError())
                        {
                            api = await initApi(token);
                            wasAbleToRefresh = true;
                        }
                    }
                    if (!wasAbleToRefresh)
                    {
                        api = null;
                        settings.accessToken = null;
                        await SaveSettings();
                    }
                    
                } else {
                    if (playback.IsPlaying)
                    {
                        setSongInfo(playback);
                    }
                }
            }
            // Si rien n'a encore été initialisé ou que le refresh du token a échoué
            if (api == null && (settings.accessToken == null || settings.accessToken == String.Empty) && !browserOpen) {
                this.auth = new AuthorizationCodeAuth(clientId, clientSecret, "http://localhost:4002", "http://localhost:4002",
                        Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative 
                        | Scope.UserReadCurrentlyPlaying | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);
                this.auth.AuthReceived += AuthOnAuthReceived;
                auth.Start();
                auth.OpenBrowser();
                browserOpen = true;
            }
            // Si l'API n'a pas encore été initialisé, mais qu'un token est enrengistré
            if (api == null && settings.accessToken != String.Empty && settings.accessToken != null)
            {
                api = new SpotifyWebAPI
                {
                    AccessToken = settings.accessToken,
                    TokenType = settings.tokenType
                };
            }
        }

        private async void setSongInfo(PlaybackContext playback)
        {
            var name = playback.Item.Name;
            var currentSongId = playback.Item.Id;
            if (currentSongId != songId)
            {
                songId = currentSongId;
                var images = api.GetPlayback().Item.Album.Images;
                var image = images.Count > 0 ? images[0] : null;
                if (image != null)
                {
                    using (WebClient client = new WebClient())
                    {
                        using (Stream s = client.OpenRead(image.Url))
                        {
                            var albumImage = System.Drawing.Image.FromStream(s);

                            var resizedImage = GraphicsTools.ResizeImage(albumImage, 144, 144);

                            await Connection.SetImageAsync(resizedImage);

                        }
                    }
                }
            }
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion


    }
} */