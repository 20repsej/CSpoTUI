
using Terminal.Gui;
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Unosquare.Swan;
using System.Diagnostics;


//public List<string> colors = new List<string>();
namespace CSpoTUI
{
    
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

         //List of the users playlist names
        private static List<string> PlaylistsList = new List<string>();
        private static List<string> PlaylistsListID = new List<string>(); //List of the users playlist ids
        private static List<string> LibraryList = new List<string>(); //List of libraryplaylist names
        private static List<string> LibraryListID = new List<string>(); //List of libraryplaylist ids
        private static List<string> DeviceList = new List<string>(); //List of device names
        private static List<string> DeviceListID = new List<string>(); //List of device ids
        private static List<string> MainWindowList = new List<string>(); //List of what currently is in main windows
        private static List<string> MainWindowListID = new List<string>();

        private static float progress = 0.0f;
        private static float totalTime = 0.0f;
        private static float whatTime = 0.0f;


        // List of ids of what currently is in main windows;
        private static string deviceToPlayFrom = "";

        public Terminal.Gui.Key Key;

        static SpotifyAPI.Web.SpotifyWebAPI api;
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
            api = new SpotifyAPI.Web.SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken
            };

            await PrintUsefulData(api);


        };

            auth.Start();
            System.Console.WriteLine("Maybe wait a little");
            System.Console.WriteLine("");
            System.Console.WriteLine("Press any key to continue...");
            auth.OpenBrowser(); // Starts an internal HTTP Server

            Console.ReadLine();

            System.Console.WriteLine(PlaylistsList.Count);
            System.Console.WriteLine(DeviceList.Count);
            System.Console.WriteLine(LibraryList.Count);
            System.Console.WriteLine("Done initiating");

            GuiMain();





        }




        private static async Task PrintAllPlaylistTracks(SpotifyWebAPI api, Paging<SimplePlaylist> playlists)
        {
            if (playlists.Items == null) return;

            playlists.Items.ForEach(playlist => MainLoop.PlaylistsListID.Add(playlist.Id));
            playlists.Items.ForEach(playlist => MainLoop.PlaylistsList.Add(playlist.Name));

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

            var SearchWin = new SearchWindow()
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(100),
                Height = Dim.Percent(10)
            };

            var SearchText = new TextField(""){
                Width = Dim.Fill(),
                Height = Dim.Fill()
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



            var MainWinWin = new MainWindowKey()
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
            var MainListWin = new ListView(MainWindowList)
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


            var ProgressSong = new ProgressBar() { X = 1, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() }; //Example progressbar




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

            PlaylistsWin.Enter_Pressed += () =>
            {
                MainWindowList.Clear();
                MainWindowListID.Clear();
                PrivateProfile profile = api.GetPrivateProfile();

                string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;

                int sak = PlaylistListWin.SelectedItem;

                Paging<PlaylistTrack> playlist = api.GetPlaylistTracks(profile.Id, PlaylistsListID[sak], "");
                playlist.Items.ForEach(track => MainWindowList.Add(track.Track.Name));
                playlist.Items.ForEach(track => MainWindowListID.Add(track.Track.Id));



                MainListWin.SetSource(MainWindowList);
                MainWinWin.SetNeedsDisplay();
            };



            LibraryWin.Enter_Pressed += () =>
            {
                MainWindowList.Clear();
                MainWindowListID.Clear();
                PrivateProfile profile = api.GetPrivateProfile();

                string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;
                // When Enter is pressed this code runs
                //  int pos = PlaylistsList.FindIndex(s => s == PlaylistListWin.SelectedItem.ToString());
                int sak = LibraryListWin.SelectedItem;
                // System.Console.WriteLine(sak);

                Paging<PlaylistTrack> playlist = api.GetPlaylistTracks(profile.Id, LibraryListID[sak], "");
                playlist.Items.ForEach(track => MainWindowList.Add(track.Track.Name));
                playlist.Items.ForEach(track => MainWindowListID.Add(track.Track.Id));


                //   MainListWin.Redraw(MainListWin.Bounds);
                MainListWin.SetSource(MainWindowList);
                MainWinWin.SetNeedsDisplay();


            };
            MainWinWin.Enter_Pressed += () =>
            {
                PrivateProfile profile = api.GetPrivateProfile();

                string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;
                // When Enter is pressed this code runs
                //  int pos = PlaylistsList.FindIndex(s => s == PlaylistListWin.SelectedItem.ToString());
                int sak = MainListWin.SelectedItem;
                string song = "spotify:track:" + MainWindowListID[sak];

                //api.ResumePlayback(deviceId: "", contextUri: "spotify:track:" + MainWindowListID[sak], uris: null, "", 0);
                api.ResumePlayback(
                    uris: new List<string>() { song },
                    positionMs: 0,
                    offset: 0
                );
                
            };
            SearchWin.Enter_Pressed += () =>
            {
                MainWindowList.Clear();
                MainWindowListID.Clear();
                int sak = MainListWin.SelectedItem;
                 Paging<FullTrack> item = api.SearchItems(SearchText.Text.ToString(), SearchType.Track).Tracks ;
                item.Items.ForEach(track => MainWindowList.Add(track.Name));
                item.Items.ForEach(track => MainWindowListID.Add(track.Id));

                MainWinWin.SetNeedsDisplay();
            };

            Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(1000), x => { 
                PlaybackContext context = api.GetPlayback();
                if (context.Item != null)
                {
                    totalTime = context.Item.DurationMs;
                    whatTime = context.ProgressMs;
                }

                progress = whatTime/totalTime;
                ProgressSong.Fraction = progress;

               // System.Console.WriteLine(context.ProgressMs);
                return true;
                           
            });
            



            /// <summary>
            /// Adds all the functions to the right place and window
            /// Also adds the windows to the main window
            /// </summary>
            SearchWin.Add(SearchText);
            DeviceDialog.Add(DeviceListWin, current);
            PlayerWin.Add(ProgressSong);
            MainWinWin.Add(MainListWin);
            LibraryWin.Add(LibraryListWin);
            PlaylistsWin.Add(PlaylistListWin);
            MainWindow.Add(menu, SearchWin, LibraryWin, PlaylistsWin, MainWinWin, PlayerWin);
            top.Add(MainWindow);
            
            Application.Run(top); // Starts the GUI
        }

    }

    /// <summary>
    /// Takes the key <c>Key.Enter</c> as input for the playlist window
    /// </summary>
    class PlaylistWindow : Window
    {
        public Action Enter_Pressed;

        public PlaylistWindow() : base("Playlists")
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

    /// <summary>
    /// Takes the key <c>Key.Enter</c> as input for the library window
    /// </summary>
    class LibraryWindow : Window
    {
        public Action Enter_Pressed;

        public LibraryWindow() : base("Library")
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
    class MainWindowKey : Window
    {
        public Action Enter_Pressed;

        public MainWindowKey() : base("CSpoTUI")
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

    /// <summary>
    /// Takes the key <c>Key.Enter</c> as input for the search window
    /// </summary>
    class SearchWindow : Window
    {
        public Action Enter_Pressed;

        public SearchWindow() : base("Search")
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

