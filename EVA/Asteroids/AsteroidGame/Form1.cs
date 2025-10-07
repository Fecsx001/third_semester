using AsteroidGame.Model;
using AsteroidGame.Persistence;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AsteroidGame
{
    public partial class Form1 : Form
    {
        private GameModel _gameModel;
        private GamePersistence _persistence;
        private System.Windows.Forms.Timer _gameTimer;
        private DateTime _lastUpdateTime;
        private bool _leftKeyPressed = false;
        private bool _rightKeyPressed = false;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameModel = new GameModel(ClientSize.Width, ClientSize.Height);
            _persistence = new GamePersistence();
            
            _gameModel.GameOver += GameModel_GameOver;
            _gameModel.ScoreChanged += GameModel_ScoreChanged;
            _gameModel.GameTimeChanged += GameModel_GameTimeChanged;

            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16; // ~60 FPS
            _gameTimer.Tick += GameTimer_Tick;
            _lastUpdateTime = DateTime.Now;
            _gameTimer.Start();

            DoubleBuffered = true;
            UpdateStatus();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsed = currentTime - _lastUpdateTime;
            _lastUpdateTime = currentTime;

            if (_leftKeyPressed) _gameModel.MoveSpaceshipLeft();
            if (_rightKeyPressed) _gameModel.MoveSpaceshipRight();
            
            _gameModel.Update(elapsed);
            Invalidate();
        }

        private void GameModel_ScoreChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void GameModel_GameTimeChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void GameModel_GameOver(object sender, EventArgs e)
        {
            _gameTimer.Stop();
            UpdateStatus();
            Invalidate();
            
            MessageBox.Show($"Game Over!\nTime: {_gameModel.GameTime:mm\\:ss}\nScore: {_gameModel.Score}", 
                "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatus));
                return;
            }

            string status = $"Asteroid Game - Time: {_gameModel.GameTime:mm\\:ss} - Score: {_gameModel.Score}";
            
            if (_gameModel.IsGameOver)
                status += " - GAME OVER";
            else if (_gameModel.IsPaused)
                status += " - PAUSED";
            else
                status += " - Playing";
            
            Text = status;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            // Draw stars in background
            DrawStars(g);

            // Draw spaceship
            DrawSpaceship(g);

            // Draw asteroids
            DrawAsteroids(g);

            // Draw UI text
            DrawUIText(g);
        }

        private void DrawStars(Graphics g)
        {
            Random rand = new Random(1); // Fixed seed for consistent stars
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(ClientSize.Width);
                int y = rand.Next(ClientSize.Height);
                g.FillRectangle(Brushes.White, x, y, 2, 2);
            }
        }

        private void DrawSpaceship(Graphics g)
        {
            var ship = _gameModel.Spaceship;
            
            // Ship body
            g.FillRectangle(Brushes.LightBlue, ship.X, ship.Y, ship.Width, ship.Height);
            g.DrawRectangle(Pens.White, ship.X, ship.Y, ship.Width, ship.Height);
            
            // Ship cockpit
            g.FillRectangle(Brushes.Blue, ship.X + 10, ship.Y - 10, ship.Width - 20, 10);
            g.DrawRectangle(Pens.White, ship.X + 10, ship.Y - 10, ship.Width - 20, 10);
            
            // Ship engines
            g.FillRectangle(Brushes.Orange, ship.X + 15, ship.Y + ship.Height, ship.Width - 30, 10);
        }

        private void DrawAsteroids(Graphics g)
        {
            foreach (var asteroid in _gameModel.Asteroids)
            {
                // Asteroid with rocky texture
                g.FillEllipse(Brushes.Gray, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                g.DrawEllipse(Pens.DarkGray, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                
                // Asteroid details
                g.FillEllipse(Brushes.DarkGray, asteroid.X + 10, asteroid.Y + 10, 8, 8);
                g.FillEllipse(Brushes.DarkGray, asteroid.X + 25, asteroid.Y + 20, 5, 5);
                g.FillEllipse(Brushes.DarkGray, asteroid.X + 15, asteroid.Y + 25, 6, 6);
            }
        }

        private void DrawUIText(Graphics g)
        {
            if (_gameModel.IsGameOver)
            {
                DrawCenteredText(g, "GAME OVER", Brushes.Red, 32);
                DrawCenteredText(g, $"Time: {_gameModel.GameTime:mm\\:ss}  Score: {_gameModel.Score}", Brushes.White, 16, 40);
                DrawCenteredText(g, "Press SPACE to play again", Brushes.Yellow, 14, 80);
            }
            else if (_gameModel.IsPaused)
            {
                DrawCenteredText(g, "PAUSED", Brushes.Yellow, 32);
                DrawCenteredText(g, "Press SPACE to resume", Brushes.White, 14, 40);
            }
        }

        private void DrawCenteredText(Graphics g, string text, Brush brush, int fontSize, int offsetY = 0)
        {
            using (var font = new Font("Arial", fontSize, FontStyle.Bold))
            {
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, brush, 
                    (ClientSize.Width - size.Width) / 2, 
                    (ClientSize.Height - size.Height) / 2 + offsetY);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    _leftKeyPressed = true;
                    break;
                case Keys.Right:
                    _rightKeyPressed = true;
                    break;
                case Keys.Space:
                    if (_gameModel.IsGameOver)
                    {
                        NewGame();
                    }
                    else
                    {
                        _gameModel.TogglePause();
                        UpdateStatus();
                        Invalidate();
                    }
                    break;
                case Keys.Escape:
                    Application.Exit();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    _leftKeyPressed = false;
                    break;
                case Keys.Right:
                    _rightKeyPressed = false;
                    break;
            }
        }

        private void NewGame()
        {
            _gameModel.InitializeGame();
            _lastUpdateTime = DateTime.Now;
            _gameTimer.Start();
            UpdateStatus();
            Invalidate();
        }

        private void SaveGame()
        {
            if (_gameModel.IsPaused && !_gameModel.IsGameOver)
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Asteroid Game (*.save)|*.save";
                    saveDialog.DefaultExt = "save";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            _persistence.SaveGame(saveDialog.FileName, _gameModel);
                            MessageBox.Show("Game saved successfully!", "Save Game", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error saving game: {ex.Message}", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please pause the game before saving.", "Save Game", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadGame()
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Asteroid Game (*.save)|*.save";
                openDialog.DefaultExt = "save";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var gameData = _persistence.LoadGame(openDialog.FileName);
                        
                        // Convert AsteroidData to Asteroid objects
                        var asteroids = new List<Asteroid>();
                        foreach (var asteroidData in gameData.Asteroids)
                        {
                            asteroids.Add(new Asteroid(asteroidData.X, asteroidData.Y, gameData.ScreenHeight));
                        }
                        
                        _gameModel.SetGameState(gameData.Score, gameData.GameTime, gameData.SpaceshipX, asteroids);
                        UpdateStatus();
                        Invalidate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading game: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Menu event handlers
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e) => NewGame();
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameModel.TogglePause();
            UpdateStatus();
            Invalidate();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) => SaveGame();
        private void loadToolStripMenuItem_Click(object sender, EventArgs e) => LoadGame();
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }
}