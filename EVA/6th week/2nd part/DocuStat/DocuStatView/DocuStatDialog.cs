using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ELTE.DocuStat.Model;
using ELTE.DocuStat.Persistence;

namespace ELTE.DocuStat.View
{
    public partial class DocuStatDialog : Form
    {
        public DocuStatDialog()
        {
            InitializeComponent();

            openFileDialogMenuItem.Click += OpenDialog;
            countWordsMenuItem.Click += CountWords;
        }

        private async void OpenDialog(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Text files (*.txt)|*.txt|PDF files (*.pdf)|*.pdf";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var tasks = new List<Task>();
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        var task = AddTabPageAsync(fileName);
                        tasks.Add(task);
                    }
                    await Task.WhenAll(tasks);
                }
            }
        }

        private async Task AddTabPageAsync(string fileName)
        {
            IFileManager? fileManager = FileManagerFactory.CreateForPath(fileName);

            if (fileManager == null)
            {
                MessageBox.Show("File reading is unsuccessful!\nUnsupported file format.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DocuStatControl control = new DocuStatControl();
                var task =  control.LoadFileAsync(fileManager);
                TabPage tabPage = new TabPage("Loading...");
                tabPage.Controls.Add(control);
                tabControl.TabPages.Add(tabPage);

                await task;
                tabPage.Text = System.IO.Path.GetFileName(fileName);
            }
            catch (FileManagerException ex)
            {
                MessageBox.Show("File reading is unsuccessful!\n" + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CountWords(object? sender, EventArgs e)
        {
           
        }
    }
}