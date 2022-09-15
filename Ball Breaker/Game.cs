namespace Ball_Breaker
{
    public class Game
    {
        private const int sizeInCells = 12;

        private Cell[,] cells;

        public Game(int sizeInPixels)
        {
            cells = new Cell[sizeInCells, sizeInCells];

            int cellSizeInPixels = sizeInPixels / sizeInCells;
            for (int x = 0; x < sizeInCells; x++)
                for (int y = 0; y < sizeInCells; y++)
                    cells[x, y] = new Cell(new Rectangle(x * cellSizeInPixels, y * cellSizeInPixels,
                                                            cellSizeInPixels, cellSizeInPixels));
        }

        public void Draw(Graphics graphics)
        {
            foreach (Cell cell in cells)
            {
                cell.Draw(graphics);
            }
        }
    }
}
