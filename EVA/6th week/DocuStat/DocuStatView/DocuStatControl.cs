﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ELTE.DocuStat.Model;
using ELTE.DocuStat.Persistence;
using System.Threading.Tasks;

namespace ELTE.DocuStat.View
{
    public partial class DocuStatControl : UserControl
    {
        private IDocumentStatistics? _documentStatistics;

        private readonly string _prefixCharacters;
        private readonly string _prefixNonWhitespaceCharacters;
        private readonly string _prefixSentences;
        private readonly string _prefixProperNouns;
        private readonly string _prefixColemanLieuIndex;
        private readonly string _prefixFleschReadingEase;

        public DocuStatControl()
        {
            InitializeComponent();

            _prefixCharacters = labelCharacters.Text;
            _prefixNonWhitespaceCharacters = labelNonWhitespaceCharacters.Text;
            _prefixSentences = labelSentences.Text;
            _prefixProperNouns = labelProperNouns.Text;
            _prefixColemanLieuIndex = labelColemanLieuIndex.Text;
            _prefixFleschReadingEase = labelFleschReadingEase.Text;
        }

        public async Task LoadFileAsync(IFileManager fileManager)
        {
            _documentStatistics = new DocumentStatistics(fileManager);
            _documentStatistics.FileContentReady += UpdateFileContent;
            _documentStatistics.TextStatisticsReady += UpdateTextStatistics;
            await _documentStatistics.LoadAsync();
        }

        private void UpdateFileContent(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateFileContent(sender, e));
                return;
            }

            if (_documentStatistics?.FileContent == textBox.Text)
                return;

            textBox.Text = _documentStatistics?.FileContent;
            listBoxCounter.Items.Clear();
        }

        private void UpdateTextStatistics(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() => UpdateTextStatistics(sender, e));
                return;
            }

            labelCharacters.Text = $"{_prefixCharacters} {_documentStatistics!.CharacterCount}";
            labelNonWhitespaceCharacters.Text = $"{_prefixNonWhitespaceCharacters} {_documentStatistics.NonWhiteSpaceCharacterCount}";
            labelSentences.Text = $"{_prefixSentences} {_documentStatistics.SentenceCount}";
            labelProperNouns.Text = $"{_prefixProperNouns} {_documentStatistics.ProperNounCount}";
            labelColemanLieuIndex.Text = $"{_prefixColemanLieuIndex} {_documentStatistics.ColemanLieuIndex:F2}";
            labelFleschReadingEase.Text = $"{_prefixFleschReadingEase} {_documentStatistics.FleschReadingEase:F2}";
        }

        public void CalculateStatistics()
        {
            if (_documentStatistics == null || string.IsNullOrEmpty(_documentStatistics.FileContent))
            {
                MessageBox.Show("No file is loaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int minLength = Convert.ToInt32(spinBoxMinLength.Value);
            int minOccurrence = Convert.ToInt32(spinBoxMinOccurrence.Value);

            var ignoredWords = textBoxIgnoredWords.Text.Split(',')
                .Select(w => w.Trim().ToLower())
                .ToList() ?? new List<string>();

            var pairs = _documentStatistics.DistinctWordCount
                .Where(p => p.Value >= minOccurrence)
                .Where(p => p.Key.Length >= minLength)
                .Where(p => !ignoredWords.Contains(p.Key))
                .OrderByDescending(p => p.Value);

            listBoxCounter.Items.Clear();
            listBoxCounter.BeginUpdate();
            foreach (var pair in pairs)
            {
                listBoxCounter.Items.Add(pair.Key + ": " + pair.Value);
            }
            listBoxCounter.EndUpdate();
        }
    }
}