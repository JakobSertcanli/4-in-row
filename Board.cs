using System;
using Gtk;
using Window = Gtk.Window;
using Color = Cairo.Color;

class Board
{
    public readonly int rows, cols;
    ICell[,] board;
    public bool IsFull {
        get
        {
            for(int i = 0; i < cols; i++)
                if(board[0,i].IsEmpty)
                    return false;
            return true;
        }
    }
    
    public Board(int rows, int cols)
    {   
        if(rows <= 0 || cols <= 0)
            throw new ArgumentException("rows and cols must have a positive value");
        
        this.rows = rows;
        this.cols = cols;
    
        board = new Cell[rows,cols];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                board[i,j] = new Cell();
            }
        }
    }
    public bool IsColumnFull(int col)  
    { 
        if(col < 0 || col >= cols) 
            throw new ArgumentOutOfRangeException();

        return board[0,col].State != CellState.Empty;}
    public bool DropPiece(int col, CellState player)
    {
        if(col < 0 || col >= cols)
            return false;
        if(IsColumnFull(col))
            return false;

        for(int i = rows-1; i >= 0; i--)
        {
            if(board[i,col].IsEmpty)
            {
                board[i,col].PlacePiece(player);

                return true;
            }
        }
        return false;
    }
    public CellState[,] GetBoard()
    {
        CellState[,] states = new CellState[rows,cols];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                states[i,j] = board[i,j].State;
            }
        }
        return states;
    }
}