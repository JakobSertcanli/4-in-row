using System;
using Gtk;
using Color = Cairo.Color;
using Window = Gtk.Window;

interface IRenderer 
{
    Grid grid {get;}
    void Display();
    void SetBoard(Board board);
}

class Renderer : IRenderer
{
    public Grid grid {get;}
    private Board? board;
    readonly Dictionary<CellState, Color> colors = new()
    {
        {CellState.Red, new Color(1,0,0,1)},
        {CellState.Yellow, new Color(1,1,0,1)}
    };
    public Renderer(int rows, int cols)
    {
        if(rows <= 0 || cols <= 0)
            throw new ArgumentException("Rows and columns must be a positive integer.");
        grid = new Grid(rows, cols, 100, new Color(0.7,0.7,0.7,1), new Color(0.5,0.5,0.5,1));
    }
    public void SetBoard(Board board)
    {
        this.board = board;
    }
    public void Display()
    {
        if (board == null) 
            return;

        grid.Clear();
        CellState[,] states = board.GetBoard();
        for(int i = 0; i < board.rows; i++)
        {
            for(int j = 0; j < board.cols; j++)
            {
                if(states[i,j] != CellState.Empty)
                    grid.AddShape(new Circle(j,i,colors[states[i,j]]));
            }
        }
        grid.QueueDraw();
    }
}