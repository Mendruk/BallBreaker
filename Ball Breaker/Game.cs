namespace Ball_Breaker;

public class Game
{
    private readonly int cellSizeInPixels;
    private readonly int sizeInCells;

    private readonly Cell[,] cells;
    private readonly List<Cell> selectedCells = new();

    private Queue<Task> tasksQueue = new();

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

            MoveBallsVerticallyIntoEmptyCells();
            MoveBallsHorizontallyIntoEmptyCells();
            FillEmptyCellsColumns();

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

    public void RememberEachBallColor()
    {
        CanUndo = true;

        foreach (Cell cell in cells)
            cell.RememberBallColor();
    }

    public void UndoPastTurn()
    {
        CanUndo = false;
        selectedCells.Clear();

        foreach (Cell cell in cells)
            cell.ReturnPastBallColor();
    }

    private void MoveBallsVerticallyIntoEmptyCells()
    {
        List<Cell> cellsToMove = cells
            .OfType<Cell>()
            .Where(cell => cell.Y + 1 < sizeInCells &&
                           cell.BallColor != BallColors.None &&
                           cells[cell.X, cell.Y + 1].BallColor == BallColors.None)
            .ToList();

        if (cellsToMove.Count == 0)
            return;

        foreach (Cell cellToMove in cellsToMove)
        {
            cells[cellToMove.X, cellToMove.Y + 1].BallColor = cellToMove.BallColor;
            cellToMove.BallColor = BallColors.None;
        }

        MoveBallsVerticallyIntoEmptyCells();
    }

    private void MoveBallsHorizontallyIntoEmptyCells()
    {
        List<Cell> cellsToMove = cells
            .OfType<Cell>()
            .Where(cell => cell.X + 1 < sizeInCells &&
                           cell.BallColor != BallColors.None &&
                           cells[cell.X + 1, cell.Y].BallColor == BallColors.None)
            .ToList();

        if (cellsToMove.Count == 0)
            return;

        foreach (Cell cellToMove in cellsToMove)
        {
            cells[cellToMove.X + 1, cellToMove.Y].BallColor = cellToMove.BallColor;
            cellToMove.BallColor = BallColors.None;
        }

        MoveBallsHorizontallyIntoEmptyCells();
    }

    private void FillEmptyCellsColumns()
    {
        for (int x = 0; x < sizeInCells; x++)
        {
            IEnumerable<Cell> cellsColumn = cells
                .OfType<Cell>()
                .Where(cell => cell.X == x);

            if (cellsColumn.All(cell => cell.BallColor == BallColors.None))
                foreach (Cell cell in cellsColumn)
                    cell.BallColor = Cell.GetRandomBallColor();
        }

        MoveBallsVerticallyIntoEmptyCells();
        MoveBallsHorizontallyIntoEmptyCells();
    }
}