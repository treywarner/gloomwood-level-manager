using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gloomwood_Level_Manager
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings(); // Load settings when the window is initialized
        }

        private void LoadSettings()
        {
            levelsDirectoryTextBox.Text = Properties.Settings.Default.LevelsDirectory;
            gloomDirectoryTextBox.Text = Properties.Settings.Default.GloomDirectory;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(gloomDirectoryTextBox.Text) && File.Exists(gloomDirectoryTextBox.Text + "/Gloomwood.exe"))
            {
                Properties.Settings.Default.LevelsDirectory = levelsDirectoryTextBox.Text;
                Properties.Settings.Default.GloomDirectory = gloomDirectoryTextBox.Text;
                Properties.Settings.Default.Save(); // Save the settings
                Close(); // Close the settings window
            } else
            {
                System.Windows.Forms.MessageBox.Show("Invalid Gloom Directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BrowseGloomButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    gloomDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void BrowseLevelButton_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    levelsDirectoryTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the settings window without saving
        }
    }
}
