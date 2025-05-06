using System.ComponentModel.DataAnnotations.Schema;
using Gtk;

interface IWinCondition
{
    bool WinConditionMeet(int column, CellState player, Board board);
}
class FourInRow : IWinCondition
{
    public bool WinConditionMeet(int column, CellState player, Board board)
    {
        if(column < 0 || column >= board.cols)
            throw new ArgumentOutOfRangeException();
        CellState [,] states = board.GetBoard();
        int currentRow = 0;

        for (int i = 0; i < board.rows; i++)
        {
            if(states[i, column] != CellState.Empty){
                currentRow = i;
                break;
            }
        }
        if(CheckVertical(column,player,states,board))
            return true;
        else if(CheckHorizontal(column,currentRow, player,states,board))
            return true;
        else if(CheckDiagLR(column,currentRow, player,states,board))
            return true;
        else if(CheckDiagRL(column,currentRow, player,states,board))
            return true;
        return false;
    }

    protected bool CheckVertical(int column, CellState player, CellState[,] states, Board board)
    {
        int counter = 0;
        for (int i = 0; i < board.rows; i++)
        {
            if(states[i,column] == player)
                counter++;
            else
                counter = 0;

             if(counter >= 4)
                return true;
        }
        return false;
    }
    protected bool CheckHorizontal(int column, int currentRow, CellState player, CellState[,] states, Board board)
    {
        int counter = 0;
        for (int i = 0; i < board.cols; i++)
        {
           if(states[currentRow, i] == player)
               counter++;
           else
               counter = 0;

            if(counter >= 4)
                return true;
        } 
        return false;
    }
    protected bool CheckDiagLR(int column, int currentRow, CellState player, CellState[,] states, Board board)
    {
        int counter = 0, borderI = 0, borderJ = 0;

        for(int i = currentRow, j = column; i > -1 && j > -1; j--, i--)
       {
           borderI = i;
           borderJ = j;
       }

       
       for(int i = borderI, j =borderJ; i < board.rows && j < board.cols; i++, j++)
       {
           if(states[i,j] == player)        
               counter++;
           else
               counter = 0;

            if(counter >= 4)
                return true;
       }
       return false;
    }
    protected bool CheckDiagRL(int column, int currentRow, CellState player, CellState[,] states, Board board)
    {
        int counter = 0, borderI = 0, borderJ = 0;
        for (int i = currentRow, j = column; i > -1 && j < board.cols; i--, j++)
       {
           borderI = i;
           borderJ = j;
       }
       for(int i = borderI, j = borderJ; i < board.rows && j > -1; i++, j--)
       {
           if(states[i,j] == player)
               counter++;
           else
               counter = 0;

            if(counter >= 4)
                return true;
       }
        return false;
    }
}