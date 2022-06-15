using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using WheelTTSLinesCreatorLibrary;

namespace WheelTTSLinesCreatorUI
{
    public partial class WheelTTSLinesCreatorForm : Form
    {
        public static WheelTTSLinesCreatorForm instance;
        private delegate void SafeCallDelegate(string text);

        bool isCancelled = false;

        private ITTSVoice ttsVoice = WheelTTSLinesCreatorFactory.CreateTTSVoice();
        private IAudioFileCreatorManager audioFileCreatorManager = WheelTTSLinesCreatorFactory.CreateAudioFileCreatorManager();

        public WheelTTSLinesCreatorForm()
        {
            InitializeComponent();
        }

        public void WheelTTSLinesCreatorForm_Load(object sender, EventArgs e)
        {
            if (instance == null) instance = this;

            cancelButton.Enabled = false;
            outputDirectoryTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            foreach (string voice in ttsVoice.GetVoices())
            {
                ttsComboBox.Items.Add(voice);
            }
        }

        private void ChangeTextToSpeechVoice(object sender, EventArgs e)
        {
            if (ttsComboBox.Text != string.Empty)
            {
                ttsVoice.SetVoice(ttsComboBox.Text);
            }
        }

        private void gameDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                gameDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void outputDirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                outputDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void miscLinesBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Browse For Misc. Lines Text File";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                miscLinesTextBox.Text = openFileDialog.FileName;
            }
        }

        private void bankLinesBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.Title = "Browse For Misc. Lines Text File";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bankLinesTextBox.Text = openFileDialog.FileName;
            }
        }

        private void ttsTestButton_Click(object sender, EventArgs e)
        {
            ttsVoice.SpeakToDefaultAudioDevice("Which of these are real programs on National Public Radio?");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            isCancelled = false;
            DisableUI();
            StartMakingTTSFiles();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            isCancelled = true;
            audioFileCreatorManager.CancelAudioExport();
            EnableUI();
        }

        private FilePaths SetupFilePaths()
        {
            FilePaths filePaths = new FilePaths
            {
                GamePath = gameDirectoryTextBox.Text,
                OutputPath = outputDirectoryTextBox.Text,
                MiscellaneousLinesPath = miscLinesTextBox.Text,
                BankLinesPath = bankLinesTextBox.Text
            };
            return filePaths;
        }

        private async void StartMakingTTSFiles()
        {
            if (!(tappingCheckBox.Checked || matchingCheckBox.Checked || thisThatCheckBox.Checked || enumerateCheckBox.Checked
                || guessingCheckBox.Checked || writingCheckBox.Checked || miscLinesCheckBox.Checked || bankFilesCheckBox.Checked))
            {
                MessageBox.Show("Please select a task on the right", "Wheel TTS Lines Creator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            FilePaths filePaths = SetupFilePaths();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                TextFilter.SetFilters(filterTextBox.Text);

                if (tappingCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.TappingList, new Logger());
                if (matchingCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.Matching, new Logger());
                if (thisThatCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.RapidFire, new Logger());
                if (enumerateCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.NumberTarget, new Logger());
                if (guessingCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.Guessing, new Logger());
                if (writingCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.TypingList, new Logger());
                if (miscLinesCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.MiscLines, new Logger());
                if (bankFilesCheckBox.Checked && !isCancelled) await audioFileCreatorManager.StartExportingAudioFilesAsync(filePaths, ttsVoice, QuestionType.BankLines, new Logger());
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show($"Directory not found, please ensure you have selected the correct game directory.", "Wheel TTS Lines Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show($"Error processing data: Please ensure you have selected the correct .txt file for misc/bank lines and/or have ensured the filter format is correct!", "Wheel TTS Lines Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show($"Missing or illegal file path detected. If doing Misc/Bank line audio files, please make sure to include the respective .txt files in the respective text boxes.", "Wheel TTS Lines Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Uh Oh! Something Bad Happened!\n\nAn unknown issue occured (report if this doesn't go away)\n\nDetails:\n{e}", "Wheel TTS Lines Creator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            stopwatch.Stop();
            LogLine($"Total Time: { stopwatch.Elapsed }");

            EnableUI();
        }

        public void LogLine(string line)
        {
            if (logTextBox.InvokeRequired)
            {
                SafeCallDelegate safeCallDelegate = new SafeCallDelegate(LogLine);
                logTextBox.Invoke(safeCallDelegate, new object[] { line });
            }
            else
            {
                logTextBox.AppendText($"{ Environment.NewLine }{ line }");
            }
        }

        private void DisableUI()
        {
            startButton.Enabled = false;
            cancelButton.Enabled = true;
            filterModifyButton.Enabled = false;

            tappingCheckBox.Enabled = false;
            matchingCheckBox.Enabled = false;
            thisThatCheckBox.Enabled = false;
            enumerateCheckBox.Enabled = false;
            guessingCheckBox.Enabled = false;
            writingCheckBox.Enabled = false;
            miscLinesCheckBox.Enabled = false;
            bankFilesCheckBox.Enabled = false;

            gameDirectoryTextBox.Enabled = false;
            gameDirectoryBrowseButton.Enabled = false;
            outputDirectoryTextBox.Enabled = false;
            outputDirectoryBrowseButton.Enabled = false;
            miscLinesTextBox.Enabled = false;
            miscLinesBrowseButton.Enabled = false;
            bankLinesTextBox.Enabled = false;
            bankLinesBrowseButton.Enabled = false;

            ttsComboBox.Enabled = false;
            ttsTestButton.Enabled = false;
        }

        private void EnableUI()
        {
            startButton.Enabled = true;
            cancelButton.Enabled = false;
            filterModifyButton.Enabled = true;

            tappingCheckBox.Enabled = true;
            matchingCheckBox.Enabled = true;
            thisThatCheckBox.Enabled = true;
            enumerateCheckBox.Enabled = true;
            guessingCheckBox.Enabled = true;
            writingCheckBox.Enabled = true;
            miscLinesCheckBox.Enabled = true;
            bankFilesCheckBox.Enabled = true;

            gameDirectoryTextBox.Enabled = true;
            gameDirectoryBrowseButton.Enabled = true;
            outputDirectoryTextBox.Enabled = true;
            outputDirectoryBrowseButton.Enabled = true;
            miscLinesTextBox.Enabled = true;
            miscLinesBrowseButton.Enabled = true;
            bankLinesTextBox.Enabled = true;
            bankLinesBrowseButton.Enabled = true;

            ttsComboBox.Enabled = true;
            ttsTestButton.Enabled = true;
        }

        private void filterModifyButton_Click(object sender, EventArgs e)
        {
            if (filterTextBox.Enabled)
            {
                filterTextBox.Enabled = false;
                filterModifyButton.Text = "Modify";
                startButton.Enabled = true;
            }
            else
            {
                filterTextBox.Enabled = true;
                filterModifyButton.Text = "Done";
                startButton.Enabled = false;
            }
        }

        private void outputDirectoryLabel_Click(object sender, EventArgs e)
        {

        }

        private void miscLinesFileLabel_Click(object sender, EventArgs e)
        {

        }

        private void bankLinesLabel_Click(object sender, EventArgs e)
        {

        }

        private void filtersLabel_Click(object sender, EventArgs e)
        {

        }

        private void outputLogLabel_Click(object sender, EventArgs e)
        {

        }

        private void gameDirectoryLabel_Click(object sender, EventArgs e)
        {

        }

        private void ttsVoiceLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
