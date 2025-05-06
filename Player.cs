using Gtk;
using Gdk;

interface IPlayer
{
    int Turn(out CellState player);
    string ToString();
}

class CPUPlayer : IPlayer
{
    CellState player;
    public CPUPlayer(CellState player)
    {
        this.player = player;
    }
    public int Turn(out CellState player)
    {
        Random rnd = new();
        player = this.player;

        return rnd.Next(9);
    }
    public override string ToString()
    {
        return "Computer player";
    }
}

class KeyboardPlayer : IPlayer
{
    CellState player;
    int Move = -1;
    public KeyboardPlayer(CellState player, Gtk.Window gameWindow)
    {
        this.player = player;
        gameWindow.KeyPressEvent += OnKeyPress;
    }
    public int Turn(out CellState player)
    {
        player = this.player;
        int temp = Move;
        Move = -1;
        return temp;
    }
    public override string ToString()
    {
        return "Keyboard player";
    }
    private void OnKeyPress(object sender, KeyPressEventArgs args)
    {

        Move = -1;
        if(char.IsDigit((char)args.Event.Key))
        {  
            int col = (int)args.Event.Key-48;
            if(col >=1 && col <=9)
                Move = col-1;
        }
    }
}