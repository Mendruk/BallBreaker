using System.Drawing.Drawing2D;

namespace Ball_Breaker
{
    public partial class MainForm : Form
    {
        private const int sizeInCells = 12;
        private readonly int cellSizeInPixels;

        private Game game;

        public MainForm()
        {
            InitializeComponent();

            cellSizeInPixels = Math.Min(pictureGameField.Width, pictureGameField.Height) / sizeInCells;

            game = new Game(sizeInCells, cellSizeInPixels);
        }

        private void pictureGameField_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(e.Graphics);

            labelScore.Text = $"Score: {game.Score}";

            buttonUndo.Enabled = game.CanUndo;
        }

        private void pictureGameField_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / cellSizeInPixels;
            int y = e.Y / cellSizeInPixels;

            game.SelectBall(x, y);

            timer.Enabled = true;

            pictureGameField.Refresh();
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (!game.CanUndo)
                return;

            game.UndoPastTurn();

            pictureGameField.Refresh();
        }

        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            game.StartNewGame();
            pictureGameField.Refresh();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

        }

    }
}