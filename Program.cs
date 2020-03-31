/*using Terminal.Gui;

class Demo
{
    static void Main()
    {
        Application.Init();
        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
        });
        
                var playlist = new Window (new Rect (0, 1, 30, 4), "Playlist") {         //Cord system: x,y,lenght, height
                   // X = 0,
                   // Y = 1,
                  //  Width = Dim.Fill (),
                   // Height = Dim.Fill () - 1
                };

                var library = new Window (new Rect (0, 5, 30, 8), "Library");
                var player = new Window (new Rect (0, 5, 30, 8), "Player");
        

        void SetupMyView (View myView)
{
    var label = new Label ("Username: ") {
        X = 1,
        Y = 1,
        Width = 20,
        Height = 1
    };
    myView.Add (label);

    var username = new TextField ("") {
        X = 1,
        Y = 2,
        Width = 30,
        Height = 1
    };
    myView.Add (username);
}

        var playlist = new Label("playlist")
        {
            X = Pos.Percent(0),
            Y = 1,
            Width = Dim.Percent(50),
            Height = Dim.Percent(30)
        };
        var library = new Label("Library")
        {
            
            X = Pos.Percent(0),
            Y = Pos.Percent(40),
            Width = Dim.Percent(50),
            Height = Dim.Percent(40)
        };
        

        // Add both menu and win in a single call
        Application.Top.Add(menu, playlist, library);
        Application.Run();
    }
    
}*/
