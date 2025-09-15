namespace Quit;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        QuitButton = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // QuitButton
        // 
        QuitButton.Location = new System.Drawing.Point(267, 118);
        QuitButton.Name = "QuitButton";
        QuitButton.Size = new System.Drawing.Size(204, 112);
        QuitButton.TabIndex = 0;
        QuitButton.Text = "Quit";
        QuitButton.UseVisualStyleBackColor = true;
        QuitButton.Click += QuitButton_Click;
        QuitButton.MouseClick += ButtonClick;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(QuitButton);
        Text = "Form1";
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button QuitButton;

    #endregion
}