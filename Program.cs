using System;
using System.Runtime.InteropServices.ObjectiveC;
using Gtk;

public class Program
{
    public static void Main()
    {
        Application.Init();
        MainMenu m= new();
        Application.Run();
    }
}

class MainMenu
{
    public MainMenu()
    {
        Window menu = new("Main menu");
        menu.SetDefaultSize(500,200);
        Fixed container = new();
        Button ConnectFour = new("Connect Four");
        Button Animation = new("Animation");
        menu.DeleteEvent += (sender, args) => Application.Quit();
        ConnectFour.Clicked += (sender, args) => {StartConnectFour(); menu.Destroy();};
        Animation.Clicked += (sender, args) => {StartAnimation(); menu.Destroy();};
        container.Put(ConnectFour, 100, 100);
        container.Put(Animation, 300, 100);
        menu.Add(container);
        menu.ShowAll();
    }
    private void StartConnectFour()
    {
        GameWindow g = new(9,9);
    }
    private void StartAnimation()
    {
        GridAnimation a = new GridAnimation(9);
        a.Start();
    }
}