namespace Ball_Breaker;

public class Game
{
    private readonly int cellSizeInPixels;
    private readonly int sizeInCells;

    private readonly Cell[,] cells;
    private readonly List<Cell> selectedCells = new();

    public int Score;
    public bool CanUndo;

    public Game(int sizeInCells, int cellSizeInPixels)
    {
        this.sizeInCells = sizeInCells;
        this.cellSizeInPixels = cellSizeInPixels;

        cells = new Cell[sizeInCells, sizeInCells];

        for (int x = 0; x < sizeInCells; x++)
            for (int y = 0; y < sizeInCells; y++)
                cells[x, y] = new Cell(x, y, cellSizeInPixels);
    }

    public void Draw(Graphics graphics)
    {
        for (int x = 0; x <= sizeInCells; x++)
            graphics.DrawLine(Pens.Gray, x * cellSizeInPixels, 0, x * cellSizeInPixels, sizeInCells * cellSizeInPixels);

        for (int y = 0; y <= sizeInCells; y++)
            graphics.DrawLine(Pens.Gray, 0, y * cellSizeInPixels, sizeInCells * cellSizeInPixels, y * cellSizeInPixels);

        foreach (Cell cell in cells)
            cell.DrawBall(graphics);
    }


    public void SelectBall(int x, int y)
    {
        Cell selectedCell = cells[x, y];

        if (selectedCell == null)
            return;

        if (selectedCell.BallColor == BallColors.None)
        {
            selectedCells.Clear();
            return;
        }

        if (selectedCells.Contains(selectedCell))
        {
            CanUndo = true;

            RememberEachBallColor();

            foreach (Cell cell in selectedCells)
                cell.BallColor = BallColors.None;

            Score += CalculateScore(selectedCells.Count);
            selectedCells.Clear();

            return;
        }

        selectedCells.Clear();

        BallColors selectedBallColor = selectedCell.BallColor;

        Stack<Cell> cellsStack = new();

        cellsStack.Push(selectedCell);

        while (cellsStack.Count > 0)
        {
            Cell popCell = cellsStack.Pop();

            selectedCells.Add(popCell);

            foreach (Cell cellToPush in EnumerateAdjacentCells(popCell))
                if (cellToPush.BallColor == selectedBallColor &&
                    !selectedCells.OfType<Cell>().Contains(cellToPush))
                    cellsStack.Push(cellToPush);
        }

        if (selectedCells.Count == 1)
            selectedCells.Clear();
    }

    private IEnumerable<Cell> EnumerateAdjacentCells(Cell selectedBall)
    {
        int x = selectedBall.X;
        int y = selectedBall.Y;

        if (x + 1 < sizeInCells)
            yield return cells[x + 1, y];

        if (x - 1 >= 0)
            yield return cells[x - 1, y];

        if (y + 1 < sizeInCells)
            yield return cells[x, y + 1];

        if (y - 1 >= 0)
            yield return cells[x, y - 1];
    }

    private int CalculateScore(int ballCount)
    {
        return ballCount * (ballCount - 1);
    }

    public void StartNewGame()
    {
        CanUndo = false;
        Score = 0;

        foreach (Cell cell in cells)
            cell.BallColor = Cell.GetRandomBallColor();
    }

    public void RememberEachBallColor()
    {
        CanUndo = true;

        foreach (Cell cell in cells)
            cell.RememberBallColor();
    }

    public void UndoPastTurn()
    {
        CanUndo = false;

        foreach (Cell cell in cells)
            cell.ReturnPastBallColor();
    }

}