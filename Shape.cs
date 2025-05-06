using Cairo;

public interface IShape
{
    void Draw(Context cr, int cellSize);
    bool IsValid(int rows, int cols);
}

public class Circle : IShape
{
    public int Col { get; }
    public int Row { get; }
    public Color Color { get; }

    public Circle(int col, int row, Color color)
    {
        Col = col;
        Row = row;
        Color = color;
    }

    public void Draw(Context cr, int cellSize)
    {
        if(cellSize <= 0)
            throw new ArgumentException("Cellsize must be a positive integer");
        cr.SetSourceRGBA(Color.R, Color.G, Color.B, Color.A);
        cr.Arc(Col * cellSize + cellSize / 2.0, Row * cellSize + cellSize / 2.0, cellSize / 2.5, 0, 2 * Math.PI);
        cr.Fill();
    }

    public bool IsValid(int rows, int cols) => Col >= 0 && Col < cols && Row >= 0 && Row < rows;
}