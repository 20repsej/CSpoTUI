using Terminal.Gui;

class Demo {
    static void Main ()
    {
        Application.Init ();
        var menu = new MenuBar (new MenuBarItem [] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => { 
                    Application.RequestStop (); 
                })
            }),
        });

        var win = new Window ("Hello") {
            X = 0,
            Y = 1,
            Width = Dim.Fill (),
            Height = Dim.Fill () - 1
        };

        // Add both menu and win in a single call
        Application.Top.Add (menu, win);
        Application.Run ();
    }
}