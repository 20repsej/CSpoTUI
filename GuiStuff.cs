using Terminal.Gui;
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;

//public List<string> colors = new List<string>();
/// <summary>
/// The class containing the Main function that runs the program
/// Connects to spotify with SpotifyAPI and authenticates with <c>_clientId</c> and <c>_secretId</c>
/// Starts the GUI with <c>GuiMain()</c>
/// </summary>
class MainLoop
{

    /// <summary>
    /// Lists that is accesible everywhere
    /// Containing data from spotify API
    /// </summary>
    private static List<string> PlaylistsList = new List<string>(); //List of the users playlist names
    private static List<string> PlaylistsListID = new List<string>(); //List of the users playlist ids
    private static List<string> LibraryList = new List<string>(); //List of libraryplaylist names
    private static List<string> LibraryListID = new List<string>(); //List of libraryplaylist ids
    private static List<string> DeviceList = new List<string>(); //List of device names
    private static List<string> DeviceListID = new List<string>(); //List of device ids
    private static List<string> MainWindowList = new List<string>(); //List of what currently is in main windows
    private static List<string> MainWindowListID = new List<string>(); // List of ids of what currently is in main windows
    private static string deviceToPlayFrom = "";

    public Terminal.Gui.Key Key;


    private static string _clientId = "39b4c97ab78345f6a465eadad7d5c1ef"; //"";
    private static string _secretId = "35f4a854686545c8abf0ffcc9aaf1dd1"; //"";
    private static SpotifyAPI.Web.SpotifyWebAPI _spotify;
    public static void Main(string[] args)
    {
        _clientId = string.IsNullOrEmpty(_clientId) ?
          Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") :
          _clientId;

        _secretId = string.IsNullOrEmpty(_secretId) ?
          Environment.GetEnvironmentVariable("SPOTIFY_SECRET_ID") :
          _secretId;

        var auth =
          new AuthorizationCodeAuth(_clientId, _secretId, "http://localhost:4002", "http://localhost:4002",
            Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative | Scope.AppRemoteControl | Scope.UserReadPrivate
             | Scope.UserTopRead | Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserLibraryRead |
              Scope.UserReadRecentlyPlayed | Scope.Streaming | Scope.UserReadCurrentlyPlaying | Scope.UserFollowRead);

        auth.AuthReceived += async (sender, payload) =>
    {
        auth.Stop();
        Token token = await auth.ExchangeCode(payload.Code);
        SpotifyAPI.Web.SpotifyWebAPI api = new SpotifyAPI.Web.SpotifyWebAPI()
        {
            TokenType = token.TokenType,
            AccessToken = token.AccessToken
        };

       // await PrintUsefulData(api);

        if (token.IsExpired())
        {
            Token newToken = await auth.RefreshToken(token.RefreshToken);
            api.AccessToken = newToken.AccessToken;
          api.TokenType = newToken.TokenType;

        }
    };

        auth.Start();
        auth.OpenBrowser(); // Starts an internal HTTP Server

        //Console.ReadLine();

        //System.Console.WriteLine(PlaylistsList.Count);
        //System.Console.WriteLine(DeviceList.Count);
        //System.Console.WriteLine(LibraryList.Count);
        //System.Console.WriteLine("Done initiating");
    }

	private void testing(){
		if (SpotifyAPI.Web.SpotifyWebAPI api != null)
            {
                createSpotifyPlaylists();
            } else
            {
                messageLbl.Text = "Not Ready... Not Ready.. Stop That!";
            }
	}

    private static async Task PrintAllPlaylistTracks(SpotifyWebAPI api, Paging<SimplePlaylist> playlists)
    {
        if (playlists.Items == null) return;

        playlists.Items.ForEach(playlist => MainLoop.PlaylistsList.Add(playlist.Name));
        playlists.Items.ForEach(playlist => MainLoop.PlaylistsListID.Add(playlist.Id));
        if (playlists.HasNextPage())
            await PrintAllPlaylistTracks(api, await api.GetNextPageAsync(playlists));
    }

    private static void PrintAllDevices(SpotifyWebAPI api)
    {
        AvailabeDevices devices = api.GetDevices();
        if (devices.HasError())
        {
            Console.WriteLine(devices.Error.Message);
        }
        devices.Devices.ForEach(device => MainLoop.DeviceList.Add(device.Name));
        devices.Devices.ForEach(device => MainLoop.DeviceListID.Add(device.Id));
    }
    private static void PrintFeaturedPlaylists(SpotifyWebAPI api)
    {
        FeaturedPlaylists playlists = api.GetFeaturedPlaylists();

        // playlists.Playlists.Items.ForEach(playlist => MainLoop.LibraryList.Add(playlist.Name));
        // playlists.Playlists.Items.ForEach(playlist => MainLoop.LibraryListID.Add(playlist.Id));
    }


    private static async Task PrintUsefulData(SpotifyAPI.Web.SpotifyWebAPI api)
    {
        PrivateProfile profile = await api.GetPrivateProfileAsync();
        string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;
        // Console.WriteLine($"Hello there, {name}!");


        await PrintAllPlaylistTracks(api, api.GetUserPlaylists(profile.Id));
        PrintAllDevices(api);
        PrintFeaturedPlaylists(api);

    }
}