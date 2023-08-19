using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Gloomwood_Level_Manager
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        //private const string Server = "http://localhost:8080";
        private const string Server = "http://192.168.1.34:8080"; 
        public DownloadWindow()
        {
            InitializeComponent();
            LoadFolders();
        }

        private async void LoadFolders()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(Server);
                    string[] levelnames = response.Split(
                        new string[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );

                    List<Level> levels = new();
                    foreach (string levelname in levelnames)
                    {
                        levels.Add(new Level(levelname, Server + '/' + levelname + '/'));
                    }
                    folderListView.ItemsSource = levels;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void FolderListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Level selectedLevel = folderListView.SelectedItem as Level;
            readmeTextBlock.Markdown = "Awaiting Response...";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string request = selectedLevel.Url + "README";
                    string response = await client.GetStringAsync(request);
                    readmeTextBlock.Markdown = ModifyLinks(response, selectedLevel.Url);

                }
            }
            catch (Exception ex)
            {
                readmeTextBlock.Markdown = "README does not exist.";
            }
        }

        static string ModifyLinks(string input, string newPath)
        {
            string pattern = @"(!\[(.*?)\])\((.*?)\)"; // Regular expression pattern
            string replacement = @"$1(" + newPath + @"/$3)"; // Replacement pattern

            string modifiedString = Regex.Replace(input, pattern, replacement);
            return modifiedString;
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            Level selectedLevel = folderListView.SelectedItem as Level;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync(selectedLevel.Url);
                    string[] levelnames = response.Split(
                        new string[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );


                    string folderPath = System.IO.Path.Combine(Properties.Settings.Default.LevelsDirectory, selectedLevel.Name);
                    Directory.CreateDirectory(folderPath);

                    foreach (string levelname in levelnames)
                    {
                        byte[] fileBytes = await client.GetByteArrayAsync(selectedLevel.Url + levelname);
                        string filePath = System.IO.Path.Combine(folderPath, levelname);
                        File.WriteAllBytes(filePath, fileBytes);
                    }
                    MessageBox.Show($"Level '{selectedLevel.Name}' downloaded to '{folderPath}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*static string ModifyLinks(string input, string newPath)
        {
            string pattern = @"(!\[(.*?)\])\((.*?)\)"; // Regular expression pattern
            string replacement = @"$1(" + newPath + @"/$3)"; // Replacement pattern

            string modifiedString = Regex.Replace(input, pattern, replacement);
            return modifiedString;
        }

        private async void FolderListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            GitHubFolder selectedFolder = folderListView.SelectedItem as GitHubFolder;
            if (selectedFolder != null)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("YourApp"); // Add your app name or identifier
                        readmeTextBlock.Markdown = "Awaiting Response...";

                        // Step 1: GET request the URL of the folder
                        string folderResponse = await client.GetStringAsync(selectedFolder.Url);
                        List<GitHubFile> filesInFolder = JsonConvert.DeserializeObject<List<GitHubFile>>(folderResponse);

                        // Step 2: Find the element with name="README"
                        GitHubFile readmeFile = filesInFolder.Find(file => file.Name.Equals("README", StringComparison.OrdinalIgnoreCase));

                        if (readmeFile != null)
                        {
                            // Step 3: GET request the URL of the README file
                            string readmeResponse = await client.GetStringAsync(readmeFile.Url);
                            GitHubFile readmeContentFile = JsonConvert.DeserializeObject<GitHubFile>(readmeResponse);

                            // Step 4: GET request the "download_url" and put it in the readmeTextBlock
                            string readmeContent = await client.GetStringAsync(readmeContentFile.download_url);

                            string result = "";
                            int lastSlashIndex = readmeContentFile.download_url.LastIndexOf('/');

                            if (lastSlashIndex >= 0)
                            {
                                result = readmeContentFile.download_url.Substring(0, lastSlashIndex);
                            }
                            readmeContent = ModifyLinks(readmeContent, result);
                            readmeTextBlock.Markdown = readmeContent;
                        }
                        else
                        {
                            readmeTextBlock.Markdown = "README not found.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            GitHubFolder selectedFolder = folderListView.SelectedItem as GitHubFolder;
            if (selectedFolder != null)
            {
                string folderName = selectedFolder.Name;
                string downloadFolderPath = Properties.Settings.Default.LevelsDirectory; // Replace with your desired download path

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.UserAgent.ParseAdd("YourApp"); // Add your app name or identifier

                        string apiUrl = selectedFolder.Url;
                        string folderResponse = await client.GetStringAsync(apiUrl);
                        GitHubFile[] filesInFolder = JsonConvert.DeserializeObject<GitHubFile[]>(folderResponse);

                        string folderPath = System.IO.Path.Combine(downloadFolderPath, folderName);
                        Directory.CreateDirectory(folderPath);

                        foreach (GitHubFile file in filesInFolder)
                        {
                            byte[] fileBytes = await client.GetByteArrayAsync(file.download_url);
                            string filePath = System.IO.Path.Combine(folderPath, file.Name);
                            File.WriteAllBytes(filePath, fileBytes);
                        }

                        MessageBox.Show($"Level '{folderName}' downloaded to '{folderPath}'", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }

    public class GitHubFolder
    {
        public string Name { get; set; }
        public string Url { get; set; }
        // You might want to add more properties based on the API response
    }

    public class GitHubFile
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string download_url { get; set; }
        // You might want to add more properties based on the API response
    }
        */

        public class Level
        {
            public string Name { get; set; }
            public string Url { get; set; }

            public Level(string name, string url)
            {
                Name = name;
                Url = url;
            }
        }
    }
}