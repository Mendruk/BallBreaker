namespace Ball_Breaker;

public class Game
{
    private const int DelayTime = 200;
    private readonly int cellSizeInPixels;
    private readonly int sizeInCells;

    private readonly Random random = new();

    private readonly Cell[,] cells;
    private readonly List<Cell> selectedCells = new();

    private int previousScore;

    public event EventHandler Defeat = delegate { };

    public Game(int sizeInCells, int cellSizeInPixels)
    {
        this.sizeInCells = sizeInCells;
        this.cellSizeInPixels = cellSizeInPixels;

        cells = new Cell[sizeInCells, sizeInCells];

        for (int x = 0; x < sizeInCells; x++)
            for (int y = 0; y < sizeInCells; y++)
                cells[x, y] = new Cell(x, y, cellSizeInPixels);
    }

    public int Score { get; private set; }
    public bool CanUndo { get; private set; }

    public void Draw(Graphics graphics)
    {
        for (int x = 0; x <= sizeInCells; x++)
            graphics.DrawLine(Pens.Gray, x * cellSizeInPixels, 0, x * cellSizeInPixels, sizeInCells * cellSizeInPixels);

        for (int y = 0; y <= sizeInCells; y++)
            graphics.DrawLine(Pens.Gray, 0, y * cellSizeInPixels, sizeInCells * cellSizeInPixels, y * cellSizeInPixels);

        foreach (Cell cell in cells)
            cell.DrawBall(graphics);

        if (selectedCells.Count == 0)
            return;

        foreach (Cell selectedCell in selectedCells)
        {
            int x = selectedCell.X;
            int y = selectedCell.Y;

            BallColors color = selectedCell.BallColor;

            if (x + 1 < sizeInCells && cells[x + 1, y].BallColor != color || x + 1 >= sizeInCells)
                selectedCell.DrawCrossLineRelativeDirection(graphics, Direction.Right);

            if (x - 1 >= 0 && cells[x - 1, y].BallColor != color || x - 1 < 0)
                selectedCell.DrawCrossLineRelativeDirection(graphics, Direction.Left);

            if (y + 1 < sizeInCells && cells[x, y + 1].BallColor != color || y + 1 >= sizeInCells)
                selectedCell.DrawCrossLineRelativeDirection(graphics, Direction.Down); ;

            if (y - 1 >= 0 && cells[x, y - 1].BallColor != color || y - 1 < 0)
                selectedCell.DrawCrossLineRelativeDirection(graphics, Direction.Up);
        }

        selectedCells.OrderBy(cell => cell.Y)
            .ThenBy(cell => cell.X)
            .First()
            .DrawExpectedScore(graphics, CalculateScore(selectedCells.Count));
    }


    public void SelectBall(int x, int y, Action refreshGameField)
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
            DeleteSelectedBalls(refreshGameField);

            return;
        }

        selectedCells.Clear();

        SelectBallsOfSameColor(selectedCell);

        if (selectedCells.Count == 1)
            selectedCells.Clear();
    }

    private void DeleteSelectedBalls(Action refreshGameField)
    {
        CanUndo = true;

        RememberEachBallColor();

        foreach (Cell cell in selectedCells)
            cell.BallColor = BallColors.None;

        previousScore = Score;
        Score += CalculateScore(selectedCells.Count);
        selectedCells.Clear();

        MoveBallsIntoEmptyCells(0, 1);
        refreshGameField();
        Thread.Sleep(DelayTime);

        MoveBallsIntoEmptyCells(1, 0);
        refreshGameField();
        Thread.Sleep(DelayTime * 2);

        FillEmptyCellsColumns(refreshGameField);
        refreshGameField();

        if (AreDefeatConditionsMet())
            Defeat(this, EventArgs.Empty);
    }

    private void SelectBallsOfSameColor(Cell selectedCell)
    {
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
    }

    private IEnumerable<Cell> EnumerateAdjacentCells(Cell selectedCell)
    {
        int x = selectedCell.X;
        int y = selectedCell.Y;

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
        selectedCells.Clear();
        Score = 0;

        foreach (Cell cell in cells)
            cell.BallColor = Cell.GetRandomBallColor();
    }

    public void UndoPreviousTurn()
    {
        CanUndo = false;
        selectedCells.Clear();
        Score = previousScore;

        foreach (Cell cell in cells)
            cell.ReturnPreviousBallColor();
    }

    private void RememberEachBallColor()
    {
        CanUndo = true;

        foreach (Cell cell in cells)
            cell.RememberBallColor();
    }

    private void MoveBallsIntoEmptyCells(int xShiftSides, int yShiftSides)
    {
        List<Cell> cellsToMove = new();

        cellsToMove = cells
                    .OfType<Cell>()
                    .Where(cell => cell.X + xShiftSides < sizeInCells &&
                                   cell.Y + yShiftSides < sizeInCells &&
                                   cell.BallColor != BallColors.None &&
                                   cells[cell.X + xShiftSides, cell.Y + yShiftSides].BallColor == BallColors.None)
                    .ToList();

        if (cellsToMove.Count == 0)
            return;

        foreach (Cell cellToMove in cellsToMove)
        {
            cells[cellToMove.X + xShiftSides, cellToMove.Y + yShiftSides].BallColor = cellToMove.BallColor;
            cellToMove.BallColor = BallColors.None;
        }

        MoveBallsIntoEmptyCells(xShiftSides, yShiftSides);
    }

    private void FillEmptyCellsColumns(Action refreshGameField)
    {
        for (int x = 0; x < sizeInCells; x++)
        {
            if (cells[x, sizeInCells - 1].BallColor != BallColors.None)
                continue;

            int randomNumberOfBallsInColumn = random.Next(sizeInCells - 2);

            for (int y = randomNumberOfBallsInColumn; y < sizeInCells; y++)
                cells[x, y].BallColor = Cell.GetRandomBallColor();
        }

        refreshGameField();
        Thread.Sleep(DelayTime);

        MoveBallsIntoEmptyCells(1, 0);
    }

    private bool AreDefeatConditionsMet()
    {
        return cells.OfType<Cell>()
            .Where(cell => cell.BallColor != BallColors.None)
            .All(ball => EnumerateAdjacentCells(ball).All(cell => cell.BallColor != ball.BallColor));
    }
}