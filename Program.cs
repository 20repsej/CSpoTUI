/*using Terminal.Gui;

using System;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;



class CSpoTUI
{
    string[] testText = new string[] { "Sak1", "Sak2", "Sak3", "Sak4" };

    static void Main()
    {




        Application.Init();
        var top = new Toplevel(){
            X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill()
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
        

        var ListTest = new ListView(new[] { "sak", "sak2" })
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()

        };
      
        var ProgressSong = new ProgressBar(){X = 1, Y = 0, Width = 5, Height = 2};

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





}*/