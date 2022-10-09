namespace Ball_Breaker;

public class Cell
{
    private static readonly Random Random = new();

    private static readonly Pen LinePen = new(Color.Black, 2);

    private static readonly Font ScoreFont = new(FontFamily.GenericSansSerif, 8, FontStyle.Bold);
    private static readonly StringFormat Format = new();

    private static readonly Dictionary<BallColors, Brush> BallBrushes = new()
    {
        { BallColors.Red, Brushes.Red },
        { BallColors.Green, Brushes.Green },
        { BallColors.Blue, Brushes.Blue },
        { BallColors.Yellow, Brushes.Yellow },
        { BallColors.Violet, Brushes.Violet }
    };

    private readonly Rectangle ballRectangle;
    private readonly Rectangle cellRectangle;

    private BallColors previousBallColor;
    public BallColors BallColor;

    static Cell()
    {
        Format.Alignment = StringAlignment.Center;
    }

    public Cell(int x, int y, int cellSizeInPixels)
    {
        X = x;
        Y = y;

        int xInPixels = x * cellSizeInPixels;
        int yInPixels = y * cellSizeInPixels;

        int borderOffset = cellSizeInPixels / 10;

        BallColor = GetRandomBallColor();

        ballRectangle = new Rectangle(xInPixels + borderOffset, yInPixels + borderOffset,
            cellSizeInPixels - borderOffset * 2, cellSizeInPixels - borderOffset * 2);

        cellRectangle = new Rectangle(xInPixels, yInPixels, cellSizeInPixels, cellSizeInPixels);
    }

    public int X { get; }
    public int Y { get; }

    public static BallColors GetRandomBallColor()
    {
        return (BallColors)Random.Next(Enum.GetNames(typeof(BallColors)).Length - 1);
    }

    public void DrawBall(Graphics graphics)
    {
        if (BallBrushes.TryGetValue(BallColor, out Brush? brush))
            graphics.FillEllipse(brush, ballRectangle);
    }

    public void DrawCrossLineRelativeDirection(Graphics graphics, Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                graphics.DrawLine(LinePen, cellRectangle.X, cellRectangle.Bottom, cellRectangle.X, cellRectangle.Y);
                break;
            case Direction.Right:
                graphics.DrawLine(LinePen, cellRectangle.Right, cellRectangle.Y, cellRectangle.Right, cellRectangle.Bottom);
                break;
            case Direction.Up:
                graphics.DrawLine(LinePen, cellRectangle.X, cellRectangle.Y, cellRectangle.Right, cellRectangle.Y);
                break;
            case Direction.Down:
                graphics.DrawLine(LinePen, cellRectangle.Right, cellRectangle.Bottom, cellRectangle.X, cellRectangle.Bottom);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void DrawExpectedScore(Graphics graphics, int expectedScore)
    {
        int drawingX = cellRectangle.X;
        int drawingY = cellRectangle.Y;

        if (X == 0)
            drawingX += 14;
        if (Y == 0)
            drawingY += 6;

        graphics.FillEllipse(Brushes.Aqua, drawingX - 14, drawingY - 6, 30, 30);
        graphics.DrawString(expectedScore.ToString(), ScoreFont, Brushes.Black, drawingX, drawingY, Format);
    }

    public void RememberBallColor()
    {
        previousBallColor = BallColor;
    }

    public void ReturnPastBallColor()
    {
        BallColor = previousBallColor;
    }
}