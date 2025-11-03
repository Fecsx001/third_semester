using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ELTE.DocuStat.Persistence;

namespace ELTE.DocuStat.View
{
    public partial class DocuStatDialog : Form
    {
        public DocuStatDialog()
        {
            InitializeComponent();

            openFileDialogMenuItem.Click += OpenDialog;
            countWordsMenuItem.Click += CalculateStatistics;
        }

        private async void OpenDialog(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true; // Több fájl kiválasztás engedélyezése

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Menüpontok inaktívvá tétele betöltés alatt
                    openFileDialogMenuItem.Enabled = false;
                    countWordsMenuItem.Enabled = false;

                    try
                    {
                        tabControl.TabPages.Clear(); // Régi lapok törlése

                        List<Task> tasks = new List<Task>();
                        foreach (string fileName in openFileDialog.FileNames)
                        {
                            tasks.Add(AddTabPageAsync(fileName));
                        }

                        await Task.WhenAll(tasks);
                    }
                    finally
                    {
                        // Menüpontok visszakapcsolása
                        openFileDialogMenuItem.Enabled = true;
                        countWordsMenuItem.Enabled = true;
                    }
                }
            }
        }

        private async Task AddTabPageAsync(string fileName)
        {
            IFileManager? fileManager = FileManagerFactory.CreateForPath(fileName);
            if (fileManager == null)
            {
                MessageBox.Show($"File reading is unsuccessful!\nUnsupported file format: {fileName}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DocuStatControl control = new DocuStatControl();
                Task loadTask = control.LoadFileAsync(fileManager);
                TabPage tabPage = new TabPage("Loading...");
                tabPage.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                tabControl.TabPages.Add(tabPage);

                await loadTask;
                tabPage.Text = Path.GetFileName(fileName);
            }
            catch (FileManagerException ex)
            {
                MessageBox.Show($"File reading is unsuccessful!\n{fileName}\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CalculateStatistics(object? sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null)
            {
                MessageBox.Show("No tab is selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tabControl.SelectedTab.Controls[0] is DocuStatControl control)
            {
                control.CalculateStatistics();
            }
        }
    }
}