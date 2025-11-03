using AsteriodGameMechanic.Model;
using AsteriodGameMechanic.Persistence;

namespace AsteroidGame
{
    public partial class Form1 : Form
    {
        private GameModel _gameModel;
        private GamePersistence _persistence;
        private readonly HighScoreManager _highScoreManager;
        private System.Windows.Forms.Timer _gameTimer;
        private DateTime _lastUpdateTime;
        private bool _leftKeyPressed = false;
        private bool _rightKeyPressed = false;

        public Form1()
        {
            InitializeComponent();
            
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _highScoreManager = new HighScoreManager(appDirectory);
            
            int highScore = _highScoreManager.LoadHighScore();
            
            InitializeGame(highScore);
        }

        private void InitializeGame(int highScore)
        {
            _gameModel = new GameModel(ClientSize.Width, ClientSize.Height, highScore);
            _persistence = new GamePersistence();
            
            _gameModel.GameOver += GameModel_GameOver;
            _gameModel.ScoreChanged += GameModel_ScoreChanged;
            _gameModel.GameTimeChanged += GameModel_GameTimeChanged;
            _gameModel.HighScoreChanged += GameModel_HighScoreChanged;

            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16; //If my calculations are correct, this should be about 60 FPS so normal for a game
            _gameTimer.Tick += GameTimer_Tick;
            _lastUpdateTime = DateTime.Now;
            _gameTimer.Start();

            DoubleBuffered = true;
            UpdateStatus();
        }

        private void GameModel_HighScoreChanged(object sender, EventArgs e)
        {
            _highScoreManager.SaveHighScore(_gameModel.HighScore);
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

            if (_gameModel.Score > _gameModel.HighScore)
            {
                _gameModel.SetHighScore(_gameModel.Score);
            }
            
            UpdateStatus();
            Invalidate();
            
            string message = $"Game Over!\nTime: {_gameModel.GameTime:mm\\:ss}\nScore: {_gameModel.Score}";
            
            if (_gameModel.Score == _gameModel.HighScore && _gameModel.Score > 0)
            {
                message += "\n🎉 NEW HIGH SCORE! 🎉";
            }
            else if (_gameModel.HighScore > 0)
            {
                message += $"\nHigh Score: {_gameModel.HighScore}";
            }
            
            MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateStatus));
                return;
            }

            string status = $"Asteroid Game - Time: {_gameModel.GameTime:mm\\:ss} - Score: {_gameModel.Score} - High Score: {_gameModel.HighScore} - {_gameModel.GetDifficultyDescription()}";
    
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

            DrawStars(g);
            DrawSpaceship(g);
            DrawAsteroids(g);
            DrawUIText(g);
            DrawHighScore(g);
        }

        private void DrawStars(Graphics g)
        {
            Random rand = new Random(1);
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
            
            //body
            g.FillRectangle(Brushes.LightBlue, ship.X, ship.Y, ship.Width, ship.Height);
            g.DrawRectangle(Pens.White, ship.X, ship.Y, ship.Width, ship.Height);
            
            //cockpit
            g.FillRectangle(Brushes.Blue, ship.X + 10, ship.Y - 10, ship.Width - 20, 10);
            g.DrawRectangle(Pens.White, ship.X + 10, ship.Y - 10, ship.Width - 20, 10);
            
            //engines
            g.FillRectangle(Brushes.Orange, ship.X + 15, ship.Y + ship.Height, ship.Width - 30, 10);
        }

        private void DrawAsteroids(Graphics g)
        {
            foreach (var asteroid in _gameModel.Asteroids) 
            {
                Brush asteroidBrush;
                Pen outlinePen;
                int size = asteroid.Width;

                if (size >= 60) // Giant
                {
                    asteroidBrush = new SolidBrush(Color.FromArgb(80, 60, 40));
                    outlinePen = Pens.DarkRed;
                    
                    g.FillEllipse(asteroidBrush, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    g.DrawEllipse(outlinePen, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 15, asteroid.Y + 10, 12, 12);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + asteroid.Width - 20, asteroid.Y + 25, 15, 15);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 25, asteroid.Y + asteroid.Height - 20, 10, 10);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 45, asteroid.Y + 15, 8, 8);
                }
                else if (size >= 40) // Large
                {
                    asteroidBrush = new SolidBrush(Color.FromArgb(100, 80, 60));
                    outlinePen = Pens.DarkOrange;
                    
                    g.FillEllipse(asteroidBrush, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    g.DrawEllipse(outlinePen, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 10, asteroid.Y + 8, 8, 8);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + asteroid.Width - 15, asteroid.Y + 15, 10, 10);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 20, asteroid.Y + asteroid.Height - 15, 6, 6);
                }
                else if (size >= 25) // Medium
                {
                    asteroidBrush = Brushes.Gray;
                    outlinePen = Pens.White;
                    
                    g.FillEllipse(asteroidBrush, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    g.DrawEllipse(outlinePen, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 8, asteroid.Y + 5, 5, 5);
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + asteroid.Width - 10, asteroid.Y + 10, 4, 4);
                }
                else // Small
                {
                    asteroidBrush = Brushes.LightGray;
                    outlinePen = Pens.Yellow;
                    
                    g.FillEllipse(asteroidBrush, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    g.DrawEllipse(outlinePen, asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
                    
                    g.FillEllipse(Brushes.DarkSlateGray, asteroid.X + 4, asteroid.Y + 3, 2, 2);
                }
            }
        }

        private void DrawUIText(Graphics g)
        {
            if (_gameModel.IsGameOver)
            {
                DrawCenteredText(g, "GAME OVER", Brushes.Red, 32);
                DrawCenteredText(g, $"Time: {_gameModel.GameTime:mm\\:ss}  Score: {_gameModel.Score}", Brushes.White, 16, 40);
                if (_gameModel.Score == _gameModel.HighScore && _gameModel.Score > 0)
                {
                    DrawCenteredText(g, "🎉 NEW HIGH SCORE! 🎉", Brushes.Gold, 20, 70);
                }
                DrawCenteredText(g, "Press SPACE to play again", Brushes.Yellow, 14, 100);
            }
            else if (_gameModel.IsPaused)
            {
                DrawCenteredText(g, "PAUSED", Brushes.Yellow, 32);
                DrawCenteredText(g, "Press SPACE to resume", Brushes.White, 14, 40);
            }
        }

        private void DrawHighScore(Graphics g)
        {
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            {
                string highScoreText = $"High Score: {_gameModel.HighScore}";
                var size = g.MeasureString(highScoreText, font);
                
                g.DrawString(highScoreText, font, Brushes.Gold, 
                    ClientSize.Width - size.Width - 10, 30);
                
                string scoreText = $"Score: {_gameModel.Score}";
                var scoreSize = g.MeasureString(scoreText, font);
                g.DrawString(scoreText, font, Brushes.White, 
                    ClientSize.Width - scoreSize.Width - 10, 50);
                
                string difficultyText = $"Difficulty: {_gameModel.GetDifficultyDescription()}";
                if (_gameModel.GetDifficultyDescription() == "Easy")
                {
                    g.DrawString(difficultyText, font, Brushes.LightGreen, 10, 30);
                }
                else if (_gameModel.GetDifficultyDescription() == "Medium")
                {
                    g.DrawString(difficultyText, font, Brushes.Yellow, 10, 50);
                }
                else if (_gameModel.GetDifficultyDescription() == "Hard")
                {
                    g.DrawString(difficultyText, font, Brushes.Red, 10, 50);
                }
                else if (_gameModel.GetDifficultyDescription() == "Expert")
                {
                    g.DrawString(difficultyText, font, Brushes.DarkRed, 10, 50);
                }
                
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
            int highScore = _highScoreManager.LoadHighScore();
            _gameModel = new GameModel(ClientSize.Width, ClientSize.Height, highScore);
            _gameModel.GameOver += GameModel_GameOver;
            _gameModel.ScoreChanged += GameModel_ScoreChanged;
            _gameModel.GameTimeChanged += GameModel_GameTimeChanged;
            _gameModel.HighScoreChanged += GameModel_HighScoreChanged;
            
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
                    saveDialog.Title = "Save Game";
                    saveDialog.InitialDirectory = _highScoreManager.GetSaveDirectory();
                    
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
                openDialog.Title = "Load Game";
                openDialog.InitialDirectory = _highScoreManager.GetSaveDirectory();
                
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var gameData = _persistence.LoadGame(openDialog.FileName);
                        
                        var asteroids = new List<Asteroid>();
                        foreach (var asteroidData in gameData.Asteroids)
                        {
                            asteroids.Add(new Asteroid(
                                asteroidData.X, 
                                asteroidData.Y, 
                                gameData.ScreenHeight, 
                                asteroidData.BaseSize, 
                                asteroidData.Speed
                            ));
                        }
                        
                        int highScore = _highScoreManager.LoadHighScore();
                        
                        _gameModel = new GameModel(gameData.ScreenWidth, gameData.ScreenHeight, highScore);
                        _gameModel.GameOver += GameModel_GameOver;
                        _gameModel.ScoreChanged += GameModel_ScoreChanged;
                        _gameModel.GameTimeChanged += GameModel_GameTimeChanged;
                        _gameModel.HighScoreChanged += GameModel_HighScoreChanged;
                        
                        _gameModel.SetGameState(
                            gameData.Score, 
                            gameData.GameTime, 
                            gameData.SpaceshipX, 
                            asteroids
                        );
                        
                        UpdateStatus();
                        Invalidate();
                        
                        MessageBox.Show("Game loaded successfully!", "Load Game", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Save file not found!", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading game: {ex.Message}", "Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ResetHighScore()
        {
            if (MessageBox.Show("Are you sure you want to reset the high score?", "Reset High Score", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _highScoreManager.SaveHighScore(0);
                    _gameModel.SetHighScore(0);
                    UpdateStatus();
                    Invalidate();
                    MessageBox.Show("High score reset to 0!", "Reset High Score", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error resetting high score: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gameModel.TogglePause();
            UpdateStatus();
            Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        private void resetHighScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetHighScore();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string controlsInfo = 
                "🎮 GAME CONTROLS 🎮\n\n" +
                "← → Arrow Keys: Move Spaceship Left/Right\n" +
                "SPACE: Pause/Resume Game\n" +
                "ESC: Exit Game\n\n" +
                "📊 SCORING 📊\n\n" +
                "• Small Asteroids: 40-60 points (fast, hard to avoid)\n" +
                "• Medium Asteroids: 25-45 points\n" +
                "• Large Asteroids: 15-35 points\n" +
                "• Giant Asteroids: 10-25 points (slow, easy to avoid)\n\n" +
                "🎯 DIFFICULTY 🎯\n\n" +
                "• Game gets harder over time\n" +
                "• More asteroids spawn\n" +
                "• Asteroids move faster\n" +
                "• Variety of asteroid sizes increases";

            MessageBox.Show(controlsInfo, "Game Controls & Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aboutInfo = 
                "🚀 ASTEROID GAME 🚀\n\n" +
                "Navigate your spaceship through an asteroid field!\n\n" +
                "Features:\n" +
                "• Progressive difficulty system\n" +
                "• Variable asteroid sizes and speeds\n" +
                "• High score tracking\n" +
                "• Save/Load game functionality\n" +
                "• Dynamic scoring based on asteroid size\n\n" +
                "Survive as long as possible and achieve the highest score!\n\n" +
                "Good luck, pilot! 🌟";

            MessageBox.Show(aboutInfo, "About Asteroid Game", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_gameModel.Score > _gameModel.HighScore)
            {
                _highScoreManager.SaveHighScore(_gameModel.Score);
            }
            base.OnFormClosing(e);
        }
    }
}