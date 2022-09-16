namespace Ball_Breaker
{
    public class Cell
    {
        private static readonly Random random = new();
        static Dictionary<BallColors, Brush> brushes = new()
        {
            { BallColors.Red,    Brushes.Red },
            { BallColors.Green,  Brushes.Green },
            { BallColors.Blue,   Brushes.Blue },
            { BallColors.Yellow, Brushes.Yellow },
            { BallColors.Violet, Brushes.Violet }
        };

        private readonly Rectangle ballRectangle;
        private readonly Rectangle strokeRectangle;

        public BallColors BallColor;
        public int X;
        public int Y;

        public Cell(int x, int y, int cellSizeInPixels)
        {
            X = x;
            Y = y;

            int offset = cellSizeInPixels / 10;

            ballRectangle = new Rectangle(x * cellSizeInPixels + offset, y * cellSizeInPixels + offset,
                cellSizeInPixels - offset * 2, cellSizeInPixels - offset * 2);

            strokeRectangle = new Rectangle(x * cellSizeInPixels, y * cellSizeInPixels,
                cellSizeInPixels, cellSizeInPixels);

            BallColor = (BallColors)random.Next(Enum.GetNames(typeof(BallColors)).Length);
        }

        public void Draw(Graphics graphics)
        {
            if (brushes.TryGetValue(BallColor, out Brush? brash))
                graphics.FillEllipse(brash, ballRectangle);

        }

        public void DrawStroke(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(Brushes.Black, 7), strokeRectangle);
        }

        public void DrawSelection(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.White, strokeRectangle);
        }
    }
}
