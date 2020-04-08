using Terminal.Gui; //Gui dont work at this moment
using System;
using System.Threading.Tasks; //Spotify destroyed my gui
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;



class CSpoTUI
{

    private static string _clientId = ""; //"";
    private static string _secretId = ""; //"";

    string[] testText = new string[] { "Sak1", "Sak2", "Sak3", "Sak4" };

    static void Main()
    {

        _clientId = string.IsNullOrEmpty(_clientId) ?
          Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") :
          _clientId;

        _secretId = string.IsNullOrEmpty(_secretId) ?
          Environment.GetEnvironmentVariable("SPOTIFY_SECRET_ID") :
          _secretId;

        //Console.WriteLine("####### Spotify API Example #######");
        //Console.WriteLine("This example uses AuthorizationCodeAuth.");
        //Console.WriteLine(
        //   "Tip: If you want to supply your ClientID and SecretId beforehand, use env variables (SPOTIFY_CLIENT_ID and SPOTIFY_SECRET_ID)");

        var auth =
          new AuthorizationCodeAuth(_clientId, _secretId, "http://localhost:4002", "http://localhost:4002",
            Scope.PlaylistReadPrivate | Scope.PlaylistReadCollaborative);
        auth.AuthReceived += AuthOnAuthReceived;
        auth.Start();
        auth.OpenBrowser();

        Console.ReadLine();
        auth.Stop(0);


        // The shit down here did work 
        Application.Init();
        var top = new Toplevel()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        var Search = Application.Top;
        var Library = Application.Current;
        var Playlists = Application.Top;
        var MainWin = Application.Top;
        var Player = Application.Top;

        var MainWindow = new Window("CSpoTUI")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var SearchWin = new Window("Search")
        {
            X = 0,
            Y = 1,
            Width = Dim.Percent(100),
            Height = Dim.Percent(10)
        };

        var LibraryWin = new Window("Library")
        {
            X = 0,
            Y = Pos.Bottom(SearchWin),
            Width = Dim.Percent(20),
            Height = Dim.Percent(30)
        };
        var PlaylistsWin = new Window("Playlists")
        {
            X = 0,
            Y = Pos.Bottom(LibraryWin),
            Width = Dim.Percent(20),
            Height = Dim.Percent(30)
        };
        var MainWinWin = new Window("Main")
        {
            X = Pos.Right(LibraryWin),
            Y = Pos.Bottom(SearchWin),
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        var PlayerWin = new Window("Player")
        {
            X = 0,
            Y = Pos.Bottom(PlaylistsWin),
            Width = Dim.Percent(100),
            Height = Dim.Percent(30)
        };

       
        var ListTest = new ListView(new string[] { "1", "2" })
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()

        };

        var ProgressSong = new ProgressBar() { X = 1, Y = 0, Width = 5, Height = 2 };

        ProgressSong.Fraction = 5; // Example to show progressbBar


        var menu = new MenuBar(new MenuBarItem[] { //Creates menubar at top
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
        });
        PlayerWin.Add(ProgressSong);
        PlaylistsWin.Add(ListTest);
        MainWindow.Add(menu, SearchWin, LibraryWin, PlaylistsWin, MainWinWin, PlayerWin);
        top.Add(MainWindow);




        //var Progress = new ProgressBar ();

        // Player.Add(Progress);

        // var search = new Label ("Search") {X = Pos.Percent(30), Y = 1};
        // var searchText = new TextField ("Search"){X = 0, Y = Pos.Right(SearchWin), Width = Dim.Percent(100), Height = Dim.Percent(100)};

        // Search.Add(searchText);
        //  Search.ColorScheme = Colors.Dialog;
        Application.Run(top);
    }

    

    private static async void AuthOnAuthReceived(object sender, AuthorizationCode payload)
    {
        var auth = (AuthorizationCodeAuth)sender;
        auth.Stop();

        Token token = await auth.ExchangeCode(payload.Code);
        var api = new SpotifyAPI.Web.SpotifyWebAPI
        {
            AccessToken = token.AccessToken,
            TokenType = token.TokenType
        };
        
        // await PrintUsefulData(api);
    }

    private static async Task PrintAllPlaylistTracks(SpotifyAPI.Web.SpotifyWebAPI api, Paging<SimplePlaylist> playlists)
    {


        if (playlists.Items == null) return;

        playlists.Items.ForEach(playlist => Console.WriteLine($"- {playlist.Name}"));
        if (playlists.HasNextPage())
            await PrintAllPlaylistTracks(api, await api.GetNextPageAsync(playlists));

        


    }

    private static async Task PrintUsefulData(SpotifyAPI.Web.SpotifyWebAPI api)
    {
        PrivateProfile profile = await api.GetPrivateProfileAsync();
        string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;

        await PrintAllPlaylistTracks(api, api.GetUserPlaylists(profile.Id));
    }
    public string MakePlaylistString(SpotifyAPI.Web.SpotifyWebAPI api, Paging<SimplePlaylist> playlists){
        string lister = "sak, sak2, sak3,";
        
        if (playlists.Items == null) return lister;

        playlists.Items.ForEach(playlist => Console.WriteLine($"- {playlist.Name}"));



        
        return lister;
    }
}