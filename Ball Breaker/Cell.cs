namespace Ball_Breaker;

public class Cell
{
    private const int EdgesNumber = 4;

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
    private readonly Point[] points;

    private BallColors pastBallColor;
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

        points = new Point[EdgesNumber]
       {
            //top left => points[0]
            new (xInPixels, yInPixels),
            //top right => points[1]
            new (xInPixels + cellSizeInPixels, yInPixels),
            //bottom right => points[2]
            new (xInPixels + cellSizeInPixels, yInPixels + cellSizeInPixels),
            //bottom left => points[3]
            new (xInPixels, yInPixels + cellSizeInPixels)
       };
    }

    public int X { get; }
    public int Y { get; }

    public static BallColors GetRandomBallColor()
    {
        return (BallColors)Random.Next(Enum.GetNames(typeof(BallColors)).Length - 1);
    }

    public static BallColors GetRandomBallColorWithNone()
    {
        return (BallColors)Random.Next(Enum.GetNames(typeof(BallColors)).Length);
    }

    public void DrawBall(Graphics graphics)
    {
        if (BallBrushes.TryGetValue(BallColor, out Brush? brash))
            graphics.FillEllipse(brash, ballRectangle);
    }

    public void DrawCrossLineRelativeDirection(Graphics graphics, Direction direction)
    {
        Point FirstLinePoint;
        Point SecondLinePoint;

        switch (direction)
        {
            case Direction.Left:
                FirstLinePoint = points[3];
                SecondLinePoint = points[0];
                break;
            case Direction.Right:
                FirstLinePoint = points[1];
                SecondLinePoint = points[2];
                break;
            case Direction.Up:
                FirstLinePoint = points[0];
                SecondLinePoint = points[1];
                break;
            case Direction.Down:
                FirstLinePoint = points[2];
                SecondLinePoint = points[3];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        graphics.DrawLine(LinePen, FirstLinePoint, SecondLinePoint);
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
        pastBallColor = BallColor;
    }

    public void ReturnPastBallColor()
    {
        BallColor = pastBallColor;
    }
}