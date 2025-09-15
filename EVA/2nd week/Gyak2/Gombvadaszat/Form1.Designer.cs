namespace Gombvadaszat;

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
        pushButton = new System.Windows.Forms.Button();
        statusStrip = new System.Windows.Forms.StatusStrip();
        statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // pushButton
        // 
        pushButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        pushButton.Location = new System.Drawing.Point(288, 225);
        pushButton.Name = "pushButton";
        pushButton.Size = new System.Drawing.Size(189, 81);
        pushButton.TabIndex = 0;
        pushButton.Text = "Push Me";
        pushButton.UseVisualStyleBackColor = true;
        pushButton.Click += PushButton_Click;
        // 
        // statusStrip
        // 
        statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
        statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statusLabel });
        statusStrip.Location = new System.Drawing.Point(0, 587);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new System.Drawing.Size(1064, 42);
        statusStrip.TabIndex = 1;
        statusStrip.Text = "statusStrip";
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new System.Drawing.Size(259, 32);
        statusLabel.Text = "You can start the game";
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1064, 629);
        Controls.Add(statusStrip);
        Controls.Add(pushButton);
        MinimumSize = new System.Drawing.Size(500, 700);
        Text = "Button Hunter";
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.StatusStrip statusStrip;

    private System.Windows.Forms.Button pushButton;
    
    private System.Windows.Forms.ToolStripStatusLabel statusLabel;

    #endregion
}