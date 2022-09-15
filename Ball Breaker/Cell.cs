namespace Ball_Breaker
{
    public class Cell
    {
        private static readonly Random random = new();
        static  Dictionary<BallColors, Brush> brushes = new()
        {
            { BallColors.Red, System.Drawing.Brushes.Red },
            { BallColors.Green, System.Drawing.Brushes.Green },
            { BallColors.Blue, System.Drawing.Brushes.Blue },
            { BallColors.Yellow, System.Drawing.Brushes.Yellow },
            { BallColors.Violet, System.Drawing.Brushes.Violet }
        };

        private readonly Rectangle rectangle;

        public BallColors BallColor;

        public Cell(Rectangle rectangle)
        {
            this.rectangle = rectangle;

            BallColor = (BallColors)random.Next(Enum.GetNames(typeof(BallColors)).Length);
        }

        public void Draw(Graphics graphics)
        {
            if(brushes.TryGetValue(BallColor,out Brush? brash))
                graphics.FillEllipse(brash,rectangle);
            
            graphics.DrawRectangle(Pens.Gray,rectangle);
        }

    }
}
