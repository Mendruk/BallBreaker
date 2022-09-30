namespace Ball_Breaker
{
    public class Ball
    {
        private static readonly Font Font = new(FontFamily.GenericSansSerif, 7, FontStyle.Bold);
        private static readonly Random Random = new();

        static Dictionary<BallColors, Brush> BallBrushes = new()
        {
            { BallColors.Red, Brushes.Red },
            { BallColors.Green, Brushes.Green },
            { BallColors.Blue, Brushes.Blue },
            { BallColors.Yellow, Brushes.Yellow },
            { BallColors.Violet, Brushes.Violet }
        };

        public int number;

        private readonly int cellSizeInPixels;
        private readonly int offset;

        private Rectangle ballRectangle;
        private Rectangle strokeRectangle;

        private BallColors pastBallColor;
        private int pastX;
        private int pastY;

        public BallColors BallColor;
        public int X;
        public int Y;

        public Ball(int x, int y, int cellSizeInPixels, int number)
        {
            X = x;
            Y = y;
            this.number = number;
            this.cellSizeInPixels = cellSizeInPixels;

            offset = cellSizeInPixels / 10;

            RefreshRectangle();

            BallColor = GetRandomColor();
        }

        public static BallColors GetRandomColor()
        {
            return (BallColors)Random.Next(Enum.GetNames(typeof(BallColors)).Length - 1);
        }

        public void DrawBall(Graphics graphics)
        {
            if (BallBrushes.TryGetValue(BallColor, out Brush? brash))
            {
                graphics.FillEllipse(brash, ballRectangle);
            }
        }

        public void DrawStroke(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(Brushes.Black, 7), strokeRectangle);
        }

        public void DrawSelection(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.White, strokeRectangle);
        }

        public void DrawCloud(Graphics graphics, int score)
        {
            graphics.DrawString(score.ToString(), Font, Brushes.Black, strokeRectangle.X, strokeRectangle.Y);
        }

        public void RefreshRectangle()
        {
            ballRectangle = new Rectangle(X * cellSizeInPixels + offset, Y * cellSizeInPixels + offset,
                cellSizeInPixels - offset * 2, cellSizeInPixels - offset * 2);

            strokeRectangle = new Rectangle(X * cellSizeInPixels, Y * cellSizeInPixels,
                cellSizeInPixels, cellSizeInPixels);
        }

        public void RememberCurrentState()
        {
            pastBallColor = BallColor;
            pastX = X;
            pastY = Y;
        }

        public void ReturnPreviousState()
        {
            BallColor = pastBallColor;
            X = pastX;
            Y = pastY;
        }
    }
}
