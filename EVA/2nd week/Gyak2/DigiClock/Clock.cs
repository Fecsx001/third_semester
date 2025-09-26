namespace DigiClock;

public partial class Clock : UserControl
{
    
    private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    
    public Clock()
    {
        InitializeComponent();
        timer.Interval = 1000;
        timer.Start();
    }
    
    private int timeZone;
    public string City
    {
        get => CityLabel.Text;
        set => CityLabel.Text = value;
    }
    public int TimeZone
    {
        get { return timeZone; }
        set
        {
            timeZone = value;
            RefreshTime(this, EventArgs.Empty);
        }
    }
    
    private void RefreshTime(object sender, EventArgs e)
    {
        DateTime time = DateTime.Now;
        TimeLabel.Text = time
            .AddHours(TimeZone)
            .ToString(time.Second % 2 == 0 ? "HH:mm" : "HH mm");
    }
}