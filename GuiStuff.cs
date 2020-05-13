
using Terminal.Gui; // Gui.cs
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web; // SpotifyAPI-NET
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335; // Dont know
using Unosquare.Swan; // Dont know
using System.Diagnostics; // Dont know



namespace CSpoTUI
{

    /// <summary>
    /// The class containing the Main function that runs the program
    /// Connects to spotify with SpotifyAPI and authenticates with <c>_clientId</c> and <c>_secretId</c>
    /// Starts the GUI with <c>GuiMain()</c>
    /// </summary>
    class MainLoop // I dont like this
    {

        /// <summary>
        /// Lists that is accesible everywhere
        /// Containing data from spotify API
        /// </summary>

        //List of things to display
        private static List<string> PlaylistsList = new List<string>(); // List of the users playlist names
        private static List<string> PlaylistsListID = new List<string>(); //List of the users playlist IDs
        private static List<string> LibraryList = new List<string>(); //List of libraryplaylist names
        private static List<string> LibraryListID = new List<string>(); //List of libraryplaylist IDs
        private static List<string> DeviceList = new List<string>(); //List of device names
        private static List<string> DeviceListID = new List<string>(); //List of device IDs
        private static List<string> MainWindowList = new List<string>(); //List of what currently is in main windows
        private static List<string> MainWindowListID = new List<string>(); // List of IDs of whats currently is in the main window

        private static string[] CurrentlyPlayingBeforePause = new string[2]; // Saves what track and duration for playing track before pause


        private static float progress = 0.0f;
        private static float totalTime = 0.0f;
        private static float whatTime = 0.0f;

        private static string deviceToPlayFrom = "";
        private static string TrackInfoString = "Notning playing"; // Trackinfo before a track is started for the first time


        public Terminal.Gui.Key Key;

        static SpotifyAPI.Web.SpotifyWebAPI api;
        private static string _clientId = "39b4c97ab78345f6a465eadad7d5c1ef"; //"";
        private static string _secretId = "35f4a854686545c8abf0ffcc9aaf1dd1"; //"";
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

            GuiMain(); // Starts the GUI





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
            /// <summary>
            /// Sets the ColorScheme for all the parts of the program
            /// Problem: Colors looks different between VS code and "KDE Konsole".
            /// </summary>
            Colors.Base.Focus = Terminal.Gui.Attribute.Make(Color.Red, Color.DarkGray);
            Colors.Base.Normal = Terminal.Gui.Attribute.Make(Color.Cyan, Color.DarkGray);
            Colors.Base.HotFocus = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);
            Colors.Base.HotNormal = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);

            Colors.Menu.Focus = Terminal.Gui.Attribute.Make(Color.Red, Color.DarkGray);
            Colors.Menu.Normal = Terminal.Gui.Attribute.Make(Color.Cyan, Color.DarkGray);
            Colors.Menu.HotFocus = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);
            Colors.Menu.HotNormal = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);

            Colors.Dialog.Focus = Terminal.Gui.Attribute.Make(Color.Red, Color.DarkGray);
            Colors.Dialog.Normal = Terminal.Gui.Attribute.Make(Color.Cyan, Color.DarkGray);
            Colors.Dialog.HotFocus = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);
            Colors.Dialog.HotNormal = Terminal.Gui.Attribute.Make(Color.Red, Color.Gray);

            var top = new Toplevel()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var Search = Application.Top;
            var Library = Application.Top;
            var Playlists = Application.Top;
            var MainWin = Application.Top;
            var Player = Application.Top;

            var MainWindow = new MostMainMainWindowKey() //The window containing all other windows
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };




            var SearchWin = new SearchWindow() // Window containing searchbar
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(100),
                Height = Dim.Percent(10)
            };

            var SearchText = new TextField("") // Searchbar
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var LibrPlayMainWin = new Window(null, 0) // Window containing Library, Playlists and Main window
            {
                X = 0,
                Y = Pos.Bottom(SearchWin),
                Width = Dim.Fill(),
                Height = Dim.Percent(90)
            };

            var LibraryWin = new LibraryWindow() // Window containing Library
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(20),
                Height = Dim.Percent(50)
            };
            var PlaylistsWin = new PlaylistWindow() // Window containing Playlists
            {
                X = 0,
                Y = Pos.Bottom(LibraryWin),
                Width = Dim.Percent(20),
                Height = Dim.Fill()
            };



            var MainWinWin = new MainWindowKey() // Main window that display different things
            {
                X = Pos.Right(LibraryWin),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var PlayerWin = new PlayerKey() // Playerwindow containing progressbar for track Need to fix
            {
                X = 0,
                Y = Pos.Bottom(LibrPlayMainWin),
                Width = Dim.Fill(),
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

            var ok = new Button(3, 14, "Ok") // Button for device selecting
            {
                Clicked = () => { Application.RequestStop(); deviceToPlayFrom = DeviceListID[DeviceListWin.SelectedItem]; System.Console.WriteLine(deviceToPlayFrom); }
            };
            var cancel = new Button(10, 14, "Cancel")
            {
                Clicked = () => Application.RequestStop()
            };

            var current = new TextField("");
            var DeviceDialog = new Dialog("Devices", 60, 18, ok, cancel);
            DeviceListWin.SelectedChanged += () => current.Text = DeviceList[DeviceListWin.SelectedItem];


            var ProgressSong = new ProgressBar() { X = 1, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() }; // Progressbar for playing track

            var TrackInfo = new TextView()
            {
                X = 1,
                Y = 1,
                Text = TrackInfoString,
                ReadOnly = true,
                Width = Dim.Fill(),
                Height = 1
            };



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

            PlaylistsWin.Enter_Pressed += () => // Starts when enter is pressed in playlistwindow
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



            LibraryWin.Enter_Pressed += () => // Starts when enter is pressed in librarywindow
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
            MainWinWin.Enter_Pressed += () => // Starts when enter is pressed in mainwindow
            {
                PrivateProfile profile = api.GetPrivateProfile();

                string name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Id : profile.DisplayName;
                int selectedItemInMain = MainListWin.SelectedItem;
                string song = "spotify:track:" + MainWindowListID[selectedItemInMain];

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
                Paging<FullTrack> item = api.SearchItems(SearchText.Text.ToString(), SearchType.Track).Tracks;
                item.Items.ForEach(track => MainWindowList.Add(track.Name));
                item.Items.ForEach(track => MainWindowListID.Add(track.Id));

                MainWinWin.SetNeedsDisplay();
            };

            PlayerWin.Enter_Pressed += () =>
            {
                PlaybackContext context = api.GetPlayback();

                if (context.Item != null)
                {
                    api.PausePlayback();
                }

            };

            MainWindow.Space_Pressed += () =>
            {
                PlaybackContext context = api.GetPlayback();

                if (context.Item != null)
                {
                    if (context.IsPlaying == true)
                    {
                        CurrentlyPlayingBeforePause[0] = context.ProgressMs.ToString(); // Converts to string
                        CurrentlyPlayingBeforePause[1] = context.Item.Uri;

                        api.PausePlayback();
                    }
                    if (context.IsPlaying == false)
                    {
                        api.ResumePlayback(
                        uris: new List<string>() { CurrentlyPlayingBeforePause[1] },
                        positionMs: Convert.ToInt32(CurrentlyPlayingBeforePause[0]), // Converts it back to int :D
                        offset: 0
                        );
                    }

                }

            };

            Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(1000), x =>
            {
                PlaybackContext context = api.GetPlayback();
                if (context.Item != null)
                {
                    totalTime = context.Item.DurationMs;
                    whatTime = context.ProgressMs;

                    TrackInfoString = context.Item.Name + " - " + context.Item.Artists[0];
                    TrackInfo.Text = TrackInfoString;
                    TrackInfo.SetNeedsDisplay();
                }

                progress = whatTime / totalTime;
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
            PlayerWin.Add(ProgressSong, TrackInfo);
            MainWinWin.Add(MainListWin);
            LibraryWin.Add(LibraryListWin);
            PlaylistsWin.Add(PlaylistListWin);
            LibrPlayMainWin.Add(LibraryWin, PlaylistsWin, MainWinWin);
            MainWindow.Add(menu, SearchWin, LibrPlayMainWin, PlayerWin);
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

    class PlayerKey : Window
    {
        public Action Enter_Pressed;

        public PlayerKey() : base("Player")
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

    class MostMainMainWindowKey : Window // Not in use
    {
        public Action Space_Pressed;

        public MostMainMainWindowKey() : base("CSpoTUI")
        {
        }
        public override bool ProcessKey(KeyEvent keyEvent)
        {
            if (keyEvent.Key == Key.Space)
            {
                if (Space_Pressed != null)
                {
                    Space_Pressed.Invoke();
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

