using System.ComponentModel;

namespace DigiClock;

partial class Clock
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        CityLabel = new System.Windows.Forms.Label();
        TimeLabel = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // CityLabel
        // 
        CityLabel.Location = new System.Drawing.Point(57, 26);
        CityLabel.Name = "CityLabel";
        CityLabel.Size = new System.Drawing.Size(137, 52);
        CityLabel.TabIndex = 0;
        CityLabel.Text = "CityLabel";
        CityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TimeLabel
        // 
        TimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        TimeLabel.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        TimeLabel.Location = new System.Drawing.Point(57, 106);
        TimeLabel.Name = "TimeLabel";
        TimeLabel.Size = new System.Drawing.Size(137, 52);
        TimeLabel.TabIndex = 1;
        TimeLabel.Text = "00:00";
        // 
        // Clock
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(TimeLabel);
        Controls.Add(CityLabel);
        Size = new System.Drawing.Size(238, 218);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Label CityLabel;
    private System.Windows.Forms.Label TimeLabel;

    #endregion
}