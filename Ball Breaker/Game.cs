using System.Drawing.Drawing2D;

namespace Ball_Breaker;

public class Game
{
    private readonly int cellSizeInPixels;
    private readonly int sizeInCells;

    private readonly List<Ball> balls;
    private readonly List<Ball> selectedBalls;

    private int pastScore;
    public int Score;
    public bool CanUndo;

    public Game(int sizeInCells, int cellSizeInPixels)
    {
        this.sizeInCells = sizeInCells;
        this.cellSizeInPixels = cellSizeInPixels;

        selectedBalls = new List<Ball>();
        balls = new List<Ball>();

        for (int x = 0; x < sizeInCells; x++)
            for (int y = 0; y < sizeInCells; y++)
                balls.Add(new Ball(x, y, cellSizeInPixels));
    }

    public void Draw(Graphics graphics)
    {
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        for (int x = 0; x <= sizeInCells; x++)
            graphics.DrawLine(Pens.Gray, x * cellSizeInPixels, 0, x * cellSizeInPixels, sizeInCells * cellSizeInPixels);

        for (int y = 0; y <= sizeInCells; y++)
            graphics.DrawLine(Pens.Gray, 0, y * cellSizeInPixels, sizeInCells * cellSizeInPixels, y * cellSizeInPixels);

        foreach (Ball ball in selectedBalls)
            ball.DrawStroke(graphics);

        foreach (Ball ball in selectedBalls)
            ball.DrawSelection(graphics);

        foreach (Ball cell in balls)
            cell.DrawBall(graphics);

        if (selectedBalls.Count > 0)
            selectedBalls
                .OrderBy(ball => ball.X)
                .OrderBy(ball => ball.Y)
                .First()
                .DrawCloud(graphics, CalculateScore(selectedBalls.Count));
    }

    public void SelectBall(int x, int y)
    {
        Ball selectedBall = balls.Find(cell => cell.X == x && cell.Y == y);

        if (selectedBall == null)
            return;

        if (selectedBall.BallColor == BallColors.None)
        {
            selectedBalls.Clear();
            return;
        }

        if (selectedBalls.Contains(selectedBall))
        {
            CanUndo = true;

            RememberStates();

            FillInVoidsVertical(selectedBalls);
            FillInVoidsHorizontal(selectedBalls);
            FillInVoidsInColumn();

            Score += CalculateScore(selectedBalls.Count);
            selectedBalls.Clear();

            if (IsNoTurn())
            {
                MessageBox.Show("Game Over!", "Game Over");
                StartNewGame();
            }

            return;
        }

        selectedBalls.Clear();

        BallColors selectedBallColor = selectedBall.BallColor;

        Stack<Ball> ballsStack = new();

        ballsStack.Push(selectedBall);

        while (ballsStack.Count > 0)
        {
            Ball ball = ballsStack.Pop();

            selectedBalls.Add(ball);

            foreach (Ball ballToPush in EnumerateAdjacentBalls(ball).Where(ball => ball != null))
                if (ballToPush.BallColor == selectedBallColor &&
                    !selectedBalls.OfType<Ball>()
                        .Contains(ballToPush))
                    ballsStack.Push(ballToPush);
        }

        if (selectedBalls.Count == 1)
        {
            selectedBalls.Clear();
        }
    }

    private IEnumerable<Ball> EnumerateAdjacentBalls(Ball selectedBall)
    {
        //todo try remove repeat
        if (selectedBall.X + 1 < sizeInCells)
            yield return balls.Find(ball => ball.X == selectedBall.X + 1 && ball.Y == selectedBall.Y);

        if (selectedBall.X - 1 >= 0)
            yield return balls.Find(ball => ball.X == selectedBall.X - 1 && ball.Y == selectedBall.Y);

        if (selectedBall.Y + 1 < sizeInCells)
            yield return balls.Find(ball => ball.X == selectedBall.X && ball.Y == selectedBall.Y + 1);

        if (selectedBall.Y - 1 >= 0)
            yield return balls.Find(ball => ball.X == selectedBall.X && ball.Y == selectedBall.Y - 1);
    }

    private void FillInVoidsVertical(List<Ball> ballsToFill)
    {
        List<Ball> ballsToEnque = new List<Ball>();

        foreach (Ball ballToFill in ballsToFill)
        {
            ballToFill.BallColor = BallColors.None;

            List<Ball> ballsToMove = balls
                .Where(ball => ball.X == ballToFill.X &&
                               ball.Y < ballToFill.Y &&
                               ball.BallColor != BallColors.None)
                .OrderByDescending(ball => ball.Y)
                .ToList();

            ballsToEnque = ballsToMove
                .Concat(ballsToEnque)
                .ToList();

            foreach (Ball cell in ballsToMove)
                cell.Y++;

            if (ballsToMove.Count > 0)
            {
                ballToFill.Y = ballsToMove.OrderBy(ball => ball.Y).First().Y - 1;
                ballToFill.RefreshRectangle();
            }
        }

        Ball.BallQueue.Enqueue(ballsToEnque);
    }

    private void FillInVoidsHorizontal(List<Ball> ballsToFill)
    {
        List<Ball> ballsToEnque = new List<Ball>();

        foreach (Ball ballToFill in ballsToFill)
        {
            List<Ball> ballsToMove = balls
                .Where(ball => ball.Y == ballToFill.Y &&
                               ball.X < ballToFill.X)
                .OrderByDescending(ball => ball.X)
                    .ToList();
            ballsToEnque = ballsToMove.Concat(ballsToEnque)
                .ToList();

            foreach (Ball cell in ballsToMove)
                cell.X++;

            ballToFill.X = 0;
            ballToFill.RefreshRectangle();
        }
        Ball.BallQueue.Enqueue(ballsToEnque);

    }

    private void FillInVoidsInColumn()
    {
        for (int x = sizeInCells; x >= 0; x--)
        {
            List<Ball> ballsInColumn = balls.Where(ball => ball.X == x).OrderBy(ball => ball.Y)
                .ToList();

            if (ballsInColumn.Any(ball => ball.BallColor != BallColors.None))
                continue;

            foreach (Ball ballInColumn in ballsInColumn)
            {
                ballInColumn.BallColor = Ball.GetRandomColor();

                Ball ballToSwap = balls.Where(ball => ball.Y == ballInColumn.Y &&
                                                      ball.BallColor == BallColors.None)
                    .OrderBy(ball => ball.X)
                    .LastOrDefault(ballInColumn);

                if (ballToSwap == ballInColumn)
                    continue;

                ballInColumn.X = ballToSwap.X;

                ballToSwap.X = x;
                ballToSwap.RefreshRectangle();
            }

            Ball.BallQueue.Enqueue(ballsInColumn);
        }
    }

    private int CalculateScore(int ballCount)
    {
        return ballCount * (ballCount - 1);
    }

    private bool IsNoTurn()
    {
        //todo rename
        foreach (Ball notEmptyBall in balls.Where(ball => ball.BallColor != BallColors.None))
        {
            if (EnumerateAdjacentBalls(notEmptyBall).Any(ball => ball.BallColor == notEmptyBall.BallColor))
                return false;
        }

        return true;
    }

    // todo rename
    private void RememberStates()
    {
        pastScore = Score;

        foreach (Ball ball in balls)
            ball.RememberCurrentState();
    }

    //todo rename
    public void ReturnStates()
    {
        Score = pastScore;
        CanUndo = false;
        selectedBalls.Clear();

        foreach (Ball ball in balls)
        {
            ball.ReturnPreviousState();
            ball.RefreshRectangle();
        }
    }

    public void StartNewGame()
    {
        Score = 0;
        CanUndo = false;
        selectedBalls.Clear();

        foreach (Ball ball in balls)
            ball.BallColor = Ball.GetRandomColor();
    }
}