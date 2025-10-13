namespace ELTE.DocuStat.View
{
    partial class DocuStatControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            labelSentences = new System.Windows.Forms.Label();
            labelNonWhitespaceCharacters = new System.Windows.Forms.Label();
            textBoxIgnoredWords = new System.Windows.Forms.TextBox();
            spinBoxMinOccurrence = new System.Windows.Forms.NumericUpDown();
            spinBoxMinLength = new System.Windows.Forms.NumericUpDown();
            labelFleschReadingEase = new System.Windows.Forms.Label();
            labelColemanLieuIndex = new System.Windows.Forms.Label();
            labelProperNouns = new System.Windows.Forms.Label();
            labelCharacters = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            listBoxCounter = new System.Windows.Forms.ListBox();
            textBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).BeginInit();
            SuspendLayout();
            // 
            // labelSentences
            // 
            labelSentences.AutoSize = true;
            labelSentences.Location = new System.Drawing.Point(26, 395);
            labelSentences.Name = "labelSentences";
            labelSentences.Size = new System.Drawing.Size(92, 15);
            labelSentences.TabIndex = 29;
            labelSentences.Text = "Sentence count:";
            // 
            // labelNonWhitespaceCharacters
            // 
            labelNonWhitespaceCharacters.AutoSize = true;
            labelNonWhitespaceCharacters.Location = new System.Drawing.Point(26, 356);
            labelNonWhitespaceCharacters.Name = "labelNonWhitespaceCharacters";
            labelNonWhitespaceCharacters.Size = new System.Drawing.Size(154, 15);
            labelNonWhitespaceCharacters.TabIndex = 28;
            labelNonWhitespaceCharacters.Text = "Non-whitespace characters:";
            // 
            // textBoxIgnoredWords
            // 
            textBoxIgnoredWords.Location = new System.Drawing.Point(561, 392);
            textBoxIgnoredWords.Name = "textBoxIgnoredWords";
            textBoxIgnoredWords.Size = new System.Drawing.Size(195, 23);
            textBoxIgnoredWords.TabIndex = 27;
            // 
            // spinBoxMinOccurrence
            // 
            spinBoxMinOccurrence.Location = new System.Drawing.Point(635, 354);
            spinBoxMinOccurrence.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            spinBoxMinOccurrence.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinOccurrence.Name = "spinBoxMinOccurrence";
            spinBoxMinOccurrence.Size = new System.Drawing.Size(120, 23);
            spinBoxMinOccurrence.TabIndex = 26;
            spinBoxMinOccurrence.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // spinBoxMinLength
            // 
            spinBoxMinLength.Location = new System.Drawing.Point(635, 315);
            spinBoxMinLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinLength.Name = "spinBoxMinLength";
            spinBoxMinLength.Size = new System.Drawing.Size(120, 23);
            spinBoxMinLength.TabIndex = 25;
            spinBoxMinLength.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // labelFleschReadingEase
            // 
            labelFleschReadingEase.AutoSize = true;
            labelFleschReadingEase.Location = new System.Drawing.Point(239, 395);
            labelFleschReadingEase.Name = "labelFleschReadingEase";
            labelFleschReadingEase.Size = new System.Drawing.Size(115, 15);
            labelFleschReadingEase.TabIndex = 24;
            labelFleschReadingEase.Text = "Flesch Reading Ease:";
            // 
            // labelColemanLieuIndex
            // 
            labelColemanLieuIndex.AutoSize = true;
            labelColemanLieuIndex.Location = new System.Drawing.Point(239, 356);
            labelColemanLieuIndex.Name = "labelColemanLieuIndex";
            labelColemanLieuIndex.Size = new System.Drawing.Size(114, 15);
            labelColemanLieuIndex.TabIndex = 23;
            labelColemanLieuIndex.Text = "Coleman Lieu Index:";
            // 
            // labelProperNouns
            // 
            labelProperNouns.AutoSize = true;
            labelProperNouns.Location = new System.Drawing.Point(239, 317);
            labelProperNouns.Name = "labelProperNouns";
            labelProperNouns.Size = new System.Drawing.Size(110, 15);
            labelProperNouns.TabIndex = 22;
            labelProperNouns.Text = "Proper noun count:";
            // 
            // labelCharacters
            // 
            labelCharacters.AutoSize = true;
            labelCharacters.Location = new System.Drawing.Point(26, 317);
            labelCharacters.Name = "labelCharacters";
            labelCharacters.Size = new System.Drawing.Size(95, 15);
            labelCharacters.TabIndex = 21;
            labelCharacters.Text = "Character count:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(466, 395);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(86, 15);
            label3.TabIndex = 20;
            label3.Text = "Ignored words:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(466, 356);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(155, 15);
            label2.TabIndex = 19;
            label2.Text = "Minimum word occurrence:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(466, 317);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(130, 15);
            label1.TabIndex = 18;
            label1.Text = "Minimum word length:";
            // 
            // listBoxCounter
            // 
            listBoxCounter.FormattingEnabled = true;
            listBoxCounter.ItemHeight = 15;
            listBoxCounter.Location = new System.Drawing.Point(421, 14);
            listBoxCounter.Name = "listBoxCounter";
            listBoxCounter.Size = new System.Drawing.Size(381, 289);
            listBoxCounter.TabIndex = 17;
            // 
            // textBox
            // 
            textBox.Location = new System.Drawing.Point(26, 14);
            textBox.Multiline = true;
            textBox.Name = "textBox";
            textBox.ReadOnly = true;
            textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox.Size = new System.Drawing.Size(389, 289);
            textBox.TabIndex = 16;
            // 
            // DocuStatControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelSentences);
            Controls.Add(labelNonWhitespaceCharacters);
            Controls.Add(textBoxIgnoredWords);
            Controls.Add(spinBoxMinOccurrence);
            Controls.Add(spinBoxMinLength);
            Controls.Add(labelFleschReadingEase);
            Controls.Add(labelColemanLieuIndex);
            Controls.Add(labelProperNouns);
            Controls.Add(labelCharacters);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxCounter);
            Controls.Add(textBox);
            Name = "DocuStatControl";
            Size = new System.Drawing.Size(823, 431);
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelSentences;
        private System.Windows.Forms.Label labelNonWhitespaceCharacters;
        private System.Windows.Forms.TextBox textBoxIgnoredWords;
        private System.Windows.Forms.NumericUpDown spinBoxMinOccurrence;
        private System.Windows.Forms.NumericUpDown spinBoxMinLength;
        private System.Windows.Forms.Label labelFleschReadingEase;
        private System.Windows.Forms.Label labelColemanLieuIndex;
        private System.Windows.Forms.Label labelProperNouns;
        private System.Windows.Forms.Label labelCharacters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxCounter;
        private System.Windows.Forms.TextBox textBox;
    }
}
