/*using Terminal.Gui;
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;

class MyWindow : Window {
	public Action ControlB_Pressed;

	public MyWindow () : base ("Key event")
	{
	}
	public override bool ProcessKey (KeyEvent keyEvent)
	{
		if (keyEvent.Key == Key.ControlB) {
			if (ControlB_Pressed != null) {
				ControlB_Pressed.Invoke ();
				return true;
			}
		}
		return base.ProcessKey (keyEvent);
	}
}

class MainClass {
	public static void Main (string [] args)
	{
		Application.Init ();

		var myWindow = new MyWindow () {
			X = 0,
			Y = 0,
			Width = Dim.Fill (),
			Height = Dim.Fill ()
		};
        var PlaylistsWin = new MyWindow()
            {
                X = 0,
                
                Width = Dim.Percent(20),
                Height = Dim.Percent(50)
            };

		Label label = new Label (3, 2, "abcdef");
		myWindow.ControlB_Pressed += () => {
			//When the Ctrlkey+B is 'pressed' the following is executed
			if (label.Text != "abcdef") {
				label.Text = "abcdef";
			} else {
				label.Text = "012";
			}
		};

		myWindow.Add (label);
		Application.Top.Add (myWindow);
		Application.Run ();
	}
   // public event Action<int, int> TerminalSizeChanged;
   
}*/