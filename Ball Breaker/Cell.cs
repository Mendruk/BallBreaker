namespace Ball_Breaker;

public class Cell
{
    private static readonly Random Random = new();

    private static readonly Dictionary<BallColors, Brush> BallBrushes = new()
    {
        { BallColors.Red, Brushes.Red },
        { BallColors.Green, Brushes.Green },
        { BallColors.Blue, Brushes.Blue },
        { BallColors.Yellow, Brushes.Yellow },
        { BallColors.Violet, Brushes.Violet }
    };

    private readonly Rectangle cellRectangle;

    public BallColors BallColor;
    private BallColors pastBallColor;

    public Cell(int x, int y, int cellSizeInPixels)
    {
        X = x;
        Y = y;

        BallColor = GetRandomBallColor();

        int borderOffset = cellSizeInPixels / 10;
        cellRectangle = new Rectangle(x * cellSizeInPixels + borderOffset, y * cellSizeInPixels + borderOffset,
            cellSizeInPixels - borderOffset * 2, cellSizeInPixels - borderOffset * 2);
    }

    public int X { get; }
    public int Y { get; }

    public static BallColors GetRandomBallColor()
    {
        return (BallColors)Random.Next(Enum.GetNames(typeof(BallColors)).Length - 1);
    }

    public void DrawBall(Graphics graphics)
    {
        if (BallBrushes.TryGetValue(BallColor, out Brush? brash))
            graphics.FillEllipse(brash, cellRectangle);
    }

    public void RememberBallColor()
    {
        pastBallColor = BallColor;
    }

    public void ReturnPastBallColor()
    {
        BallColor = pastBallColor;
    }
}