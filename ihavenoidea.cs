/*using Terminal.Gui;
using System;
class Demo
{
    private static float test = 0.0f;

    static void Main()
    {
        Application.Init();
        var top = Application.Top;

        
        var win = new Window("Window")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };


        var ProgressSong = new ProgressBar() { X = 1, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() }; 
        

        Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(1000), x => { 
                
                ProgressSong.Fraction = test;
 
                test += 0.001f;
               // ProgressSong.Pulse();
              //  ProgressSong.SetNeedsDisplay();
                return true;                        
            });
        win.Add(ProgressSong);
        top.Add(win);
        Application.Run();
    }  
}*/