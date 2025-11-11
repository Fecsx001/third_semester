using System;
using System.Windows.Forms;
using AsteriodGameMechanic.Persistence;

namespace AsteroidGame
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            IHighScoreManager highScoreManager = new HighScoreManager(appDirectory);
            IGamePersistence persistence = new GamePersistence();
            Application.Run(new Form1(persistence, highScoreManager));
        }
    }
}