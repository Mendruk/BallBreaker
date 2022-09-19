using System.Drawing.Drawing2D;

namespace Ball_Breaker
{
    public class Game
    {
        private readonly int sizeInCells;
        private readonly int cellSizeInPixels;

        private List<Cell> cells;
        private List<Cell> selectedBalls;

        public int Score;

        public Game(int sizeInCells, int cellSizeInPixels)
        {
            this.sizeInCells = sizeInCells;
            this.cellSizeInPixels = cellSizeInPixels;

            cells = new();

            selectedBalls = new List<Cell>();

            for (int x = 0; x < sizeInCells; x++)
                for (int y = 0; y < sizeInCells; y++)
                    cells.Add(new Cell(x, y, cellSizeInPixels));
        }

        public void Draw(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            for (int x = 0; x < sizeInCells; x++)
                graphics.DrawLine(Pens.Gray, x * cellSizeInPixels, 0,
                    x * cellSizeInPixels, sizeInCells * cellSizeInPixels);

            for (int y = 0; y < sizeInCells; y++)
                graphics.DrawLine(Pens.Gray, 0, y * cellSizeInPixels,
                    sizeInCells * cellSizeInPixels, y * cellSizeInPixels);

            foreach (Cell selectedCell in selectedBalls)
                selectedCell.DrawStroke(graphics);

            foreach (Cell selectedCell in selectedBalls)
                selectedCell.DrawSelection(graphics);

            foreach (Cell cell in cells)
                cell.DrawBall(graphics);
        }

        public void SelectBall(int x, int y)
        {
            Cell selectedBall = cells.Find(cell => cell.X == x && cell.Y == y);

            if (selectedBall.BallColor == BallColors.None)
            {
                selectedBalls.Clear();
                return;
            }

            if (selectedBalls.Contains(selectedBall))
            {
                FillInVoidsVertical(selectedBalls);
                FillInVoidsHorizontal(selectedBalls);

                Score += CalculateScore(selectedBalls.Count);
                selectedBalls.Clear();

                return;
            }

            selectedBalls.Clear();

            BallColors selectedCellColor = selectedBall.BallColor;

            Stack<Cell> cellsStack = new();

            cellsStack.Push(selectedBall);

            while (cellsStack.Count > 0)
            {
                Cell ball = cellsStack.Pop();

                selectedBalls.Add(ball);

                foreach (Cell ballToPush in EnumerateAdjacentBalls(ball).Where(ball => ball != null))
                    if (ballToPush.BallColor == selectedCellColor &&
                        !selectedBalls.OfType<Cell>()
                            .Contains(ballToPush))
                        cellsStack.Push(ballToPush);
            }

            if (selectedBalls.Count == 1)
            {
                selectedBalls.Clear();
                return;
            }
        }

        private IEnumerable<Cell> EnumerateAdjacentBalls(Cell selectedCell)
        {
            //todo
            if (selectedCell.X + 1 < sizeInCells)
                yield return cells.Find(cell => cell.X == selectedCell.X + 1 && cell.Y == selectedCell.Y);

            if (selectedCell.X - 1 >= 0)
                yield return cells.Find(cell => cell.X == selectedCell.X - 1 && cell.Y == selectedCell.Y);

            if (selectedCell.Y + 1 < sizeInCells)
                yield return cells.Find(cell => cell.X == selectedCell.X && cell.Y == selectedCell.Y + 1);

            if (selectedCell.Y - 1 >= 0)
                yield return cells.Find(cell => cell.X == selectedCell.X && cell.Y == selectedCell.Y - 1);
        }

        private void FillInVoidsVertical(List<Cell> cellsToFill)
        {
            foreach (Cell cellToFill in cellsToFill)
            {
                cellToFill.BallColor = BallColors.None;

                List<Cell> cellsToMove = cells
                    .Where(cell => cell.X == cellToFill.X && cell.Y < cellToFill.Y)
                    .ToList();

                foreach (Cell cell in cellsToMove)
                {
                    cell.Y++;
                    cell.RefreshRectangle();
                }

                cellToFill.Y = 0;

            }
        }
        private void FillInVoidsHorizontal(List<Cell> cellsToFill)
        {

            foreach (Cell cellToFill in cellsToFill)
            {
                if (cells.Where(cell => cell.X == cellToFill.X).All(cell => cell.BallColor == BallColors.None))
                {
                    List<Cell> cellsToMove = cells.Where(cell => cell.X == cellToFill.X).ToList();

                    foreach (Cell cell in cells.Where(cell => cell.X < cellToFill.X).ToList())
                    {
                        cell.X++;
                        cell.RefreshRectangle();
                    }

                    foreach (Cell cell in cellsToMove)
                    {
                        cell.X = 0;
                        cell.BallColor = cell.GetRandomColor();
                        cell.RefreshRectangle();
                        
                    }
                }
            }
        }
        private int CalculateScore(int ballCount)
        {
            return ballCount * (ballCount - 1);
        }
    }

}
