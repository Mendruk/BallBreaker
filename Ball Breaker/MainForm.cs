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

            cellSizeInPixels = Math.Min(pictureGameField.Width, pictureGameField.Height)/sizeInCells;

            game = new Game(sizeInCells, cellSizeInPixels);
        }

        private void pictureGameField_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }

        private void pictureGameField_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / cellSizeInPixels;
            int y = e.Y / cellSizeInPixels;

            game.SelectBall(x,y);

            labelScore.Text = $"Score: {game.Score}";

            pictureGameField.Refresh();
        }

        private void buttonCancelTurn_Click(object sender, EventArgs e)
        {

            pictureGameField.Refresh();
        }
    }
}