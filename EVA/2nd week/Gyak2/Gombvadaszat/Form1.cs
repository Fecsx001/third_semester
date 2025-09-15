namespace Gombvadaszat;

public partial class Form1 : Form
{
    private int points = 0;
    private Random generator = new Random();
    private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    private DateTime startTime;

    public Form1()
    {
        InitializeComponent();

        timer.Interval = 1000;
        timer.Tick += UpdateStatusBar;

        this.FormClosing += GameClosing;
    }

    private void UpdateStatusBar(object? sender, EventArgs e)
    {
        UpdateStatusBar();
    }

    // Helper method so we can call it both from timer and click
    private void UpdateStatusBar()
    {
        double elapsedSeconds = timer.Enabled ? (DateTime.Now - startTime).TotalSeconds : 0;
        statusLabel.Text = $"Points: {points} | Elapsed time: {elapsedSeconds:F0} sec";
    }

    private void PushButton_Click(object sender, EventArgs e)
    {
        if (!timer.Enabled)
        {
            startTime = DateTime.Now;
            timer.Start();
        }

        ++points;

        int x = ClientSize.Width - pushButton.Width;
        int y = ClientSize.Height - pushButton.Height - statusStrip.Height;
        pushButton.Location = new Point(generator.Next(x), generator.Next(y));

        // Update immediately after click
        UpdateStatusBar();
    }

    private void GameClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && timer.Enabled)
        {
            double elapsedSeconds = (DateTime.Now - startTime).TotalSeconds;
            double pushPerSeconds = points / elapsedSeconds;
            MessageBox.Show(
                $"Pushes per second: {pushPerSeconds:F2}",
                "Results",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}