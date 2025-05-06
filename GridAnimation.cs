using System;
using System.Timers;
using Timeout = GLib.Timeout;
using Gtk;
using Gdk;
using Color = Cairo.Color;
using System.Net.Http.Headers;

public class GridAnimation : Gtk.Window
{
    Grid grid;
    List<(int,int)> positions = new();
    int size;
    public GridAnimation(int size) : base("Animation")
    {
        if(size <=0)
            throw new ArgumentException("Size must have a positive value");
        grid = new(size,size, 100, new Color(0.7,0.7,0.7), new Color(0.5,0.5,0.5));
        this.size = size;
        Add(grid);
        InitPos();
        DeleteEvent += (sender, args) => Application.Quit();
    }

    private void InitPos()
    {
        positions = new();
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                positions.Add(new(i,j));
            }
        }
    }

    public void Start()
    {
        ShowAll();
        Random rnd = new();
        (int,int) place;
        Timeout.Add(300, () =>
        {
            if(positions.Count == 0)
            {
                grid.Clear();
                InitPos();
            }
            else
            {
                place = positions[rnd.Next(positions.Count)];
                positions.Remove(place);
                grid.AddShape(new Circle(place.Item1, place.Item2, new Color(1,0,0,1)));
            }
            QueueDraw();
            return true;
        });
    }

}