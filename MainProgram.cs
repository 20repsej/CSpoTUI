using Terminal.Gui;
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;

//public List<string> colors = new List<string>();
namespace CSpoTUI
{

    class MainLoop
    {

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
                 | Scope.UserTopRead | Scope.PlaylistModifyPublic | Scope.UserReadPlaybackState | Scope.UserLibraryRead | Scope.UserReadRecentlyPlayed | Scope.Streaming | Scope.UserReadCurrentlyPlaying
                 | Scope.UserFollowRead);

            auth.AuthReceived += async (sender, payload) =>
        {
            auth.Stop();
            Token token = await auth.ExchangeCode(payload.Code);
            SpotifyAPI.Web.SpotifyWebAPI api = new SpotifyAPI.Web.SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken
            };

            await PrintUsefulData(api);

        };
            auth.Start(); // Starts an internal HTTP Server
            auth.OpenBrowser();

            Console.ReadLine();
            //  auth.Stop(0);
            System.Console.WriteLine(PlaylistsList.Count);
            System.Console.WriteLine(DeviceList.Count);
            System.Console.WriteLine(LibraryList.Count);
            System.Console.WriteLine("Done initiating");

            GuiMain();





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

            playlists.Playlists.Items.ForEach(playlist => MainLoop.LibraryList.Add(playlist.Name));
            playlists.Playlists.Items.ForEach(playlist => MainLoop.LibraryListID.Add(playlist.Id));

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


        static void GuiMain()
        {

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

            var LibraryWin = new LibraryWindow()
            {
                X = 0,
                Y = Pos.Bottom(SearchWin),
                Width = Dim.Percent(20),
                Height = Dim.Percent(50)
            };
            var PlaylistsWin = new PlaylistWindow()
            {
                X = 0,
                Y = Pos.Bottom(LibraryWin),
                Width = Dim.Percent(20),
                Height = Dim.Percent(50)
            };



            var MainWinWin = new Window("Main")
            {
                X = Pos.Right(LibraryWin),
                Y = Pos.Bottom(SearchWin),
                Width = Dim.Fill(),
                Height = Dim.Percent(80)
            };
            var PlayerWin = new Window("Player")
            {
                X = 0,
                Y = Pos.Bottom(PlaylistsWin),
                Width = Dim.Percent(100),
                Height = Dim.Fill()
            };


            var PlaylistListWin = new ListView(PlaylistsList)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()


            };

            var LibraryListWin = new ListView(LibraryList)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()

            };
            var DeviceListWin = new ListView(DeviceList)
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()

            };


            var ok = new Button(3, 14, "Ok")
            {
                Clicked = () => { Application.RequestStop(); deviceToPlayFrom = DeviceList[DeviceListWin.SelectedItem]; }
            };
            var cancel = new Button(10, 14, "Cancel")
            {
                Clicked = () => Application.RequestStop()
            };

            var current = new TextField("");
            var DeviceDialog = new Dialog("Devices", 60, 18, ok, cancel);
            DeviceListWin.SelectedChanged += () => current.Text = DeviceList[DeviceListWin.SelectedItem];


            var ProgressSong = new ProgressBar() { X = 1, Y = 0, Width = 5, Height = 2 }; //Example progressbar

            ProgressSong.Fraction = 6; // Example to show progressbBar


            var menu = new MenuBar(new MenuBarItem[] { //Creates menubar at top
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => { 
                    Application.RequestStop ();
                }),
                new MenuItem ("_Device", "", () => {
                    Application.Run (DeviceDialog);
                })

            }),

        });

        // Keypresses

        PlaylistsWin.Enter_Pressed += () => {
			// When Enter is pressed this code runs

           // int pos = PlaylistsList.Find(Convert.ToString( PlaylistListWin.SelectedItem));
            PlaylistListWin.ColorScheme = Colors.Error;


        };
        LibraryWin.Enter_Pressed += () => {
			// When Enter is pressed this code runs

           // int pos = PlaylistsList.Find(Convert.ToString( PlaylistListWin.SelectedItem));
            LibraryListWin.ColorScheme = Colors.Error;


        };



            DeviceDialog.Add(DeviceListWin, current);
            PlayerWin.Add(ProgressSong);
            LibraryWin.Add(LibraryListWin);
            PlaylistsWin.Add(PlaylistListWin);
            MainWindow.Add(menu, SearchWin, LibraryWin, PlaylistsWin, MainWinWin, PlayerWin);
            top.Add(MainWindow);

            Application.Run(top);
        }

    }

    class PlaylistWindow : Window
    {
        public Action Enter_Pressed;

        public PlaylistWindow() : base ("Playlists")
        {
        }
        public override bool ProcessKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Enter)
            {
                if (Enter_Pressed != null)
                {
                    Enter_Pressed.Invoke();
                    return true;
                }
            }
            return base.ProcessKey(keyEvent);
        }
    }
    class LibraryWindow : Window
    {
        public Action Enter_Pressed;

        public LibraryWindow() : base ("Library")
        {
        }
        public override bool ProcessKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Enter)
            {
                if (Enter_Pressed != null)
                {
                    Enter_Pressed.Invoke();
                    return true;
                }
            }
            return base.ProcessKey(keyEvent);
        }
    }
    class SearchWindow : Window
    {
        public Action Enter_Pressed;

        public SearchWindow() : base ("Search")
        {
        }
        public override bool ProcessKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Enter)
            {
                if (Enter_Pressed != null)
                {
                    Enter_Pressed.Invoke();
                    return true;
                }
            }
            return base.ProcessKey(keyEvent);
        }
    }

}
