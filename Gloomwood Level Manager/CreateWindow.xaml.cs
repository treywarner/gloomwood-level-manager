using Newtonsoft.Json;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gloomwood_Level_Manager
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string title = titleTextBox.Text;
            string author = authorTextBox.Text;
            string content = contentTextBox.Text;

            string sourceFolderPath = Path.Combine(Properties.Settings.Default.GloomDirectory, "Saves/Slot_" + categoryComboBox.SelectedIndex);

            string targetFolderPath = Path.Combine(Properties.Settings.Default.LevelsDirectory, title); // Replace with the target folder path

            MessageBox.Show(sourceFolderPath, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            try
            {
                CopyFolder(sourceFolderPath, targetFolderPath);
                MessageBox.Show(sourceFolderPath, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                EditJsonFile(Path.Combine(targetFolderPath, "SaveInfo.txt"), title, author);
                File.WriteAllText(Path.Combine(targetFolderPath, "README"), content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static void CopyFolder(string sourceFolderPath, string targetFolderPath)
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                throw new DirectoryNotFoundException("Source folder not found.");
            }

            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            string[] files = Directory.GetFiles(sourceFolderPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string targetFilePath = Path.Combine(targetFolderPath, fileName);
                File.Copy(file, targetFilePath, true); // Set 'true' to overwrite existing files
            }

            string[] directories = Directory.GetDirectories(sourceFolderPath);
            foreach (string directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                string targetDirectoryPath = Path.Combine(targetFolderPath, directoryName);
                CopyFolder(directory, targetDirectoryPath);
            }
        }

        static void EditJsonFile(string filePath, string newName, string newAuthor)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("JSON file not found.");
            }

            string json = File.ReadAllText(filePath);
            dynamic jsonObject = JsonConvert.DeserializeObject(json);

            // Update or add values
            jsonObject.name = newName;
            jsonObject.author = newAuthor;

            string updatedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }
    }
}
