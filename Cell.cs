public enum CellState {Empty, Red, Yellow}
interface ICell
{
    CellState State {get;}
    bool IsEmpty {get;}
    void PlacePiece(CellState state);
}
class Cell : ICell
{
    public CellState State { get; private set;} = CellState.Empty;
    public bool IsEmpty => State == CellState.Empty;
    public void PlacePiece(CellState state)
    {
        if(IsEmpty)
            State = state;
    }
}