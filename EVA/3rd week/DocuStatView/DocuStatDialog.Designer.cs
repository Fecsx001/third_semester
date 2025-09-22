namespace DocuStatView
{
    partial class DocuStatDialog
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem();
            openFileDialogMenuItem = new ToolStripMenuItem();
            countWordsMenuItem = new ToolStripMenuItem();
            textBox = new TextBox();
            listBoxCounter = new ListBox();
            labelCharacters = new Label();
            labelNonWhitespaceCharacters = new Label();
            labelSentences = new Label();
            labelProperNouns = new Label();
            labelColemanLieuIndex = new Label();
            labelFleschReadingEase = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            spinBoxMinLength = new NumericUpDown();
            spinBoxMinOccurrence = new NumericUpDown();
            textBoxIgnoredWords = new TextBox();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(9, 3, 0, 3);
            menuStrip.Size = new Size(1143, 35);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openFileDialogMenuItem, countWordsMenuItem });
            fileMenu.Name = "fileMenu";
            fileMenu.Size = new Size(54, 29);
            fileMenu.Text = "File";
            // 
            // openFileDialogMenuItem
            // 
            openFileDialogMenuItem.Name = "openFileDialogMenuItem";
            openFileDialogMenuItem.Size = new Size(270, 34);
            openFileDialogMenuItem.Text = "Open File";
            // 
            // countWordsMenuItem
            // 
            countWordsMenuItem.Name = "countWordsMenuItem";
            countWordsMenuItem.Size = new Size(270, 34);
            countWordsMenuItem.Text = "Count words";
            // 
            // textBox
            // 
            textBox.Location = new Point(17, 70);
            textBox.Margin = new Padding(4, 5, 4, 5);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.ReadOnly = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Size = new Size(563, 519);
            textBox.TabIndex = 1;
            // 
            // listBoxCounter
            // 
            listBoxCounter.FormattingEnabled = true;
            listBoxCounter.ItemHeight = 25;
            listBoxCounter.Location = new Point(609, 35);
            listBoxCounter.Margin = new Padding(4, 5, 4, 5);
            listBoxCounter.Name = "listBoxCounter";
            listBoxCounter.Size = new Size(515, 554);
            listBoxCounter.TabIndex = 2;
            // 
            // labelCharacters
            // 
            labelCharacters.AutoSize = true;
            labelCharacters.Location = new Point(40, 632);
            labelCharacters.Margin = new Padding(4, 0, 4, 0);
            labelCharacters.Name = "labelCharacters";
            labelCharacters.Size = new Size(140, 25);
            labelCharacters.TabIndex = 3;
            labelCharacters.Text = "Character count:";
            // 
            // labelNonWhitespaceCharacters
            // 
            labelNonWhitespaceCharacters.AutoSize = true;
            labelNonWhitespaceCharacters.Location = new Point(40, 690);
            labelNonWhitespaceCharacters.Margin = new Padding(4, 0, 4, 0);
            labelNonWhitespaceCharacters.Name = "labelNonWhitespaceCharacters";
            labelNonWhitespaceCharacters.Size = new Size(228, 25);
            labelNonWhitespaceCharacters.TabIndex = 4;
            labelNonWhitespaceCharacters.Text = "Non-whitespace characters:";
            // 
            // labelSentences
            // 
            labelSentences.AutoSize = true;
            labelSentences.Location = new Point(40, 743);
            labelSentences.Margin = new Padding(4, 0, 4, 0);
            labelSentences.Name = "labelSentences";
            labelSentences.Size = new Size(137, 25);
            labelSentences.TabIndex = 5;
            labelSentences.Text = "Sentence count:";
            // 
            // labelProperNouns
            // 
            labelProperNouns.AutoSize = true;
            labelProperNouns.Location = new Point(374, 632);
            labelProperNouns.Margin = new Padding(4, 0, 4, 0);
            labelProperNouns.Name = "labelProperNouns";
            labelProperNouns.Size = new Size(165, 25);
            labelProperNouns.TabIndex = 6;
            labelProperNouns.Text = "Proper noun count:";
            // 
            // labelColemanLieuIndex
            // 
            labelColemanLieuIndex.AutoSize = true;
            labelColemanLieuIndex.Location = new Point(374, 690);
            labelColemanLieuIndex.Margin = new Padding(4, 0, 4, 0);
            labelColemanLieuIndex.Name = "labelColemanLieuIndex";
            labelColemanLieuIndex.Size = new Size(170, 25);
            labelColemanLieuIndex.TabIndex = 7;
            labelColemanLieuIndex.Text = "Coleman Lieu Index:";
            // 
            // labelFleschReadingEase
            // 
            labelFleschReadingEase.AutoSize = true;
            labelFleschReadingEase.Location = new Point(374, 743);
            labelFleschReadingEase.Margin = new Padding(4, 0, 4, 0);
            labelFleschReadingEase.Name = "labelFleschReadingEase";
            labelFleschReadingEase.Size = new Size(173, 25);
            labelFleschReadingEase.TabIndex = 8;
            labelFleschReadingEase.Text = "Flesch Reading Ease:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(680, 637);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(147, 25);
            label1.TabIndex = 9;
            label1.Text = "Minimum length:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(680, 695);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(176, 25);
            label2.TabIndex = 10;
            label2.Text = "Minimum occurence:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(680, 755);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(133, 25);
            label3.TabIndex = 11;
            label3.Text = "Ignored words:";
            // 
            // spinBoxMinLength
            // 
            spinBoxMinLength.Location = new Point(861, 633);
            spinBoxMinLength.Margin = new Padding(4, 5, 4, 5);
            spinBoxMinLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinLength.Name = "spinBoxMinLength";
            spinBoxMinLength.Size = new Size(171, 31);
            spinBoxMinLength.TabIndex = 12;
            spinBoxMinLength.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // spinBoxMinOccurrence
            // 
            spinBoxMinOccurrence.Location = new Point(859, 695);
            spinBoxMinOccurrence.Margin = new Padding(4, 5, 4, 5);
            spinBoxMinOccurrence.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinOccurrence.Name = "spinBoxMinOccurrence";
            spinBoxMinOccurrence.Size = new Size(171, 31);
            spinBoxMinOccurrence.TabIndex = 13;
            spinBoxMinOccurrence.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // textBoxIgnoredWords
            // 
            textBoxIgnoredWords.Location = new Point(811, 750);
            textBoxIgnoredWords.Margin = new Padding(4, 5, 4, 5);
            textBoxIgnoredWords.Name = "textBoxIgnoredWords";
            textBoxIgnoredWords.Size = new Size(217, 31);
            textBoxIgnoredWords.TabIndex = 14;
            // 
            // DocuStatDialog
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1143, 812);
            Controls.Add(textBoxIgnoredWords);
            Controls.Add(spinBoxMinOccurrence);
            Controls.Add(spinBoxMinLength);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(labelFleschReadingEase);
            Controls.Add(labelColemanLieuIndex);
            Controls.Add(labelProperNouns);
            Controls.Add(labelSentences);
            Controls.Add(labelNonWhitespaceCharacters);
            Controls.Add(labelCharacters);
            Controls.Add(listBoxCounter);
            Controls.Add(textBox);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "DocuStatDialog";
            Text = "Document statistics";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openFileDialogMenuItem;
        private ToolStripMenuItem countWordsMenuItem;
        private TextBox textBox;
        private ListBox listBoxCounter;
        private Label labelCharacters;
        private Label labelNonWhitespaceCharacters;
        private Label labelSentences;
        private Label labelProperNouns;
        private Label labelColemanLieuIndex;
        private Label labelFleschReadingEase;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown spinBoxMinLength;
        private NumericUpDown spinBoxMinOccurrence;
        private TextBox textBoxIgnoredWords;
    }
}
