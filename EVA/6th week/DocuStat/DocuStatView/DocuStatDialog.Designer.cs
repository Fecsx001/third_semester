using System;
using System.Windows.Forms;

namespace ELTE.DocuStat.View
{
    partial class DocuStatDialog
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openFileDialogMenuItem;
        private ToolStripMenuItem countWordsMenuItem;
        private TabControl tabControl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileMenu = new ToolStripMenuItem();
            openFileDialogMenuItem = new ToolStripMenuItem();
            countWordsMenuItem = new ToolStripMenuItem();
            tabControl = new TabControl();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            
            // menuStrip1
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(914, 30);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            
            // fileMenu
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openFileDialogMenuItem, countWordsMenuItem });
            fileMenu.Name = "fileMenu";
            fileMenu.Size = new System.Drawing.Size(46, 24);
            fileMenu.Text = "File";
            
            // openFileDialogMenuItem
            openFileDialogMenuItem.Name = "openFileDialogMenuItem";
            openFileDialogMenuItem.Size = new System.Drawing.Size(200, 26);
            openFileDialogMenuItem.Text = "Open file dialog";
            
            // countWordsMenuItem
            countWordsMenuItem.Name = "countWordsMenuItem";
            countWordsMenuItem.Size = new System.Drawing.Size(200, 26);
            countWordsMenuItem.Text = "Count words";
            
            // tabControl
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 30);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(914, 570);
            tabControl.TabIndex = 1;
            
            // DocuStatDialog
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(914, 600);
            Controls.Add(tabControl);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "DocuStatDialog";
            Text = "Document statistics";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}