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

            game.SelectCells(x,y);

            pictureGameField.Refresh();
        }
    }
}