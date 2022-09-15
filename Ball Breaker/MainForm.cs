namespace Ball_Breaker
{
    public partial class MainForm : Form
    {
        private Game game;
        public MainForm()
        {
            InitializeComponent();

            game = new Game(Math.Min(pictureGameField.Width, pictureGameField.Height));
        }

        private void pictureGameField_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }
    }
}