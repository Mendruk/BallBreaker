using System.Drawing.Drawing2D;

namespace Ball_Breaker
{
    public class Game
    {
        private readonly int sizeInCells;
        private readonly int cellSizeInPixels;

        private Cell[,] cells;
        private List<Cell> selectedCells;


        public Game(int sizeInCells, int cellSizeInPixels)
        {
            this.sizeInCells = sizeInCells;
            this.cellSizeInPixels = cellSizeInPixels;

            cells = new Cell[sizeInCells, sizeInCells];

            selectedCells = new List<Cell>();

            for (int x = 0; x < sizeInCells; x++)
                for (int y = 0; y < sizeInCells; y++)
                    cells[x, y] = new Cell(x, y, cellSizeInPixels);
        }

        public void Draw(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            for (int x = 0; x < sizeInCells; x++)
                graphics.DrawLine(Pens.Gray, x * cellSizeInPixels, 0, 
                    x * cellSizeInPixels, sizeInCells * cellSizeInPixels);

            for (int y = 0; y < sizeInCells; y++)
                graphics.DrawLine(Pens.Gray, 0, y * cellSizeInPixels, 
                    sizeInCells * cellSizeInPixels, y * cellSizeInPixels);

            foreach (Cell selectedCell in selectedCells)
                selectedCell.DrawStroke(graphics);

            foreach (Cell selectedCell in selectedCells)
                selectedCell.DrawSelection(graphics);

            foreach (Cell cell in cells)
                cell.Draw(graphics);
        }

        public void SelectCells(int x, int y)
        {
            selectedCells.Clear();

            BallColors selectedCellColor = cells[x, y].BallColor;

            Stack<Cell> cellsStack = new();

            cellsStack.Push(cells[x, y]);

            while (cellsStack.Count > 0)
            {
                Cell cell = cellsStack.Pop();

                selectedCells.Add(cell);

                foreach (Cell cellToPush in EnumerateAdjacentCells(cell))
                    if (cellToPush.BallColor == selectedCellColor &&
                        !selectedCells.OfType<Cell>()
                            .Contains(cellToPush))
                        cellsStack.Push(cellToPush);
            }
        }

        private IEnumerable<Cell> EnumerateAdjacentCells(Cell cell)
        {
            //todo
            if (cell.X + 1 < sizeInCells)
                yield return cells[cell.X + 1, cell.Y];

            if (cell.X - 1 >= 0)
                yield return cells[cell.X - 1, cell.Y];

            if (cell.Y + 1 < sizeInCells)
                yield return cells[cell.X, cell.Y + 1];

            if (cell.Y - 1 >= 0)
                yield return cells[cell.X, cell.Y - 1];
        }
    }

}
