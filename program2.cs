using Terminal.Gui;

class CSpoTUI {

    static void Main (){

        Application.Init();
        var Main = Application.Top;
        var Search = Application.Top;
        var Library = Application.Top;
        var Playlists = Application.Top;
        var MainWin = Application.Top;
        var Player = Application.Top;

        var MainWindow = new Window ("CSpoTUI"){
            X = 0,
            Y = 0,
            Width = Dim.Percent(100),
            Height = Dim.Percent(100)
        };

        var SearchWin = new Window ("Search"){
            X = 0,
            Y = 1,
            Width = Dim.Percent(100),
            Height = Dim.Percent(10)
        };

        var LibraryWin = new Window ("Library"){
            X = 0,
            Y = Pos.Bottom(SearchWin),
            Width = Dim.Percent(20),
            Height = Dim.Percent(30)
        };
        var PlaylistsWin = new Window ("Playlists"){
            X = 0,
            Y = Pos.Bottom(LibraryWin),
            Width = Dim.Percent(20),
            Height = Dim.Percent(30)
        };
        var MainWinWin = new Window ("Main"){
            X = Pos.Right(LibraryWin),
            Y = Pos.Bottom(SearchWin),
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        var PlayerWin = new Window ("Player"){
            X = 0,
            Y = Pos.Bottom(PlaylistsWin),
            Width = Dim.Percent(100),
            Height = Dim.Percent(30)
        };
        Search.Add(SearchWin);
        Library.Add(LibraryWin);
        Playlists.Add(PlaylistsWin);
        MainWin.Add(MainWinWin);
        Player.Add(PlayerWin);


        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
        });
        Main.Add(menu);

        //var Progress = new ProgressBar ();
        
       // Player.Add(Progress);

       // var search = new Label ("Search") {X = Pos.Percent(30), Y = 1};
       // var searchText = new TextField ("Search"){X = 0, Y = Pos.Right(SearchWin), Width = Dim.Percent(100), Height = Dim.Percent(100)};

       // Search.Add(searchText);
      //  Search.ColorScheme = Colors.Dialog;
        Application.Run();


    }





}