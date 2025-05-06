using System;
using Gtk;
using Cairo;
using Color = Cairo.Color;
using Gdk;
using System.Collections.Generic;

public class Grid : DrawingArea
{
    private readonly int rows, cols, cellSize;
    private readonly Color bgColor, lineColor;
    private readonly List<IShape> shapes = new();
    public Grid(int rows, int cols, int size, Color bgColor, Color lineColor)
    {
        if(rows <= 0 || cols <= 0 || size <= 0)
            throw new ArgumentException("Rows, columns and cellsize must be positive integers");

        this.rows = rows;
        this.cols = cols;
        this.cellSize = size;
        this.bgColor = bgColor;
        this.lineColor = lineColor;

        SetSizeRequest(cols * cellSize, rows * cellSize);
    }

    public void AddShape(IShape shape)
    {
        if (shape.IsValid(rows, cols))
        {
            shapes.Add(shape);
            QueueDraw();
        }
    }

    public void Clear()
    {
        shapes.Clear();
        QueueDraw();
    }

    protected override bool OnDrawn(Context cr)
    {
        DrawBackground(cr);
        DrawGridLines(cr);
        DrawShapes(cr);
        return true;
    }

    private void DrawBackground(Context cr)
    {
        cr.SetSourceColor(bgColor);
        cr.Rectangle(0, 0, cols * cellSize, rows * cellSize);
        cr.Fill();
    }

    private void DrawGridLines(Context cr)
    {
        cr.SetSourceColor(lineColor);
        for (int i = 0; i <= cols; i++)
        {
            cr.MoveTo(i * cellSize, 0);
            cr.LineTo(i * cellSize, rows * cellSize);
        }
        for (int i = 0; i <= rows; i++)
        {
            cr.MoveTo(0, i * cellSize);
            cr.LineTo(cols * cellSize, i * cellSize);
        }
        cr.Stroke();
    }

    private void DrawShapes(Context cr)
    {
        foreach (var shape in shapes)
        {
            shape.Draw(cr, cellSize);
        }
    }
}