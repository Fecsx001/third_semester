namespace Quit;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void QuitButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void ButtonClick(object sender, MouseEventArgs e)
    {
        Close();
    }
}