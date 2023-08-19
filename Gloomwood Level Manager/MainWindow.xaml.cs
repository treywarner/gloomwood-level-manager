using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Utils;
using MdXaml;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using MessageBox = System.Windows.MessageBox;

namespace Gloomwood_Level_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        public bool Levels = false;

        public MainWindow()
        {
            InitializeComponent();

            UpdateLevelList();

            if (Properties.Settings.Default.GloomDirectory == "")
                SettingsButton_Click(null,null);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadWindow downloadWindow = new DownloadWindow();
            downloadWindow.Closed += UpdateLevelListHandler;
            downloadWindow.ShowDialog();
        }

        private void UpdateLevelListHandler(object sender, EventArgs e)
        {
            UpdateLevelList();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Closed += UpdateLevelListHandler;
            settingsWindow.ShowDialog();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Closed += UpdateLevelListHandler;
            createWindow.ShowDialog();
        }

        private void LoadLaunchButton_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.Combine(Properties.Settings.Default.GloomDirectory, "Gloomwood.exe");
            LoadButton_Click(sender, e);
            Process.Start(appPath);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (levelListBox.SelectedItem is LevelItem levelItem)
            {
                string sourceFolderPath = levelItem.Path;
                string targetFolderPath = System.IO.Path.Combine(Properties.Settings.Default.GloomDirectory, "Saves/Slot_" + loadFileComboBox.SelectedIndex);

                if (Directory.Exists(sourceFolderPath))
                {
                    if (!Directory.Exists(targetFolderPath))
                    {
                        Directory.CreateDirectory(targetFolderPath);
                    }

                    string[] files = Directory.GetFiles(sourceFolderPath);
                    foreach (string file in files)
                    {
                        string fileName = System.IO.Path.GetFileName(file);
                        string targetFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                        File.Copy(file, targetFilePath, true); // Set 'true' to overwrite existing files
                    }

                    string[] directories = Directory.GetDirectories(sourceFolderPath);
                    foreach (string directory in directories)
                    {
                        string directoryName = System.IO.Path.GetFileName(directory);
                        string targetDirectoryPath = System.IO.Path.Combine(targetFolderPath, directoryName);
                        DirectoryCopy(directory, targetDirectoryPath, true);
                    }
                    bool confirm = true;
                    if (confirm)
                    {
                        MessageBox.Show("Level loaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Source folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        static string ModifyLinks(string input, string newPath)
        {
            string pattern = @"(!\[(.*?)\])\((.*?)\)"; // Regular expression pattern
            string replacement = @"$1(" + newPath + @"/$3)"; // Replacement pattern

            string modifiedString = Regex.Replace(input, pattern, replacement);
            return modifiedString;
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (levelListBox.SelectedItem != null)
            {
                // Remove the selected item from the ListBox's ItemsSource
                var selectedLevel = (LevelItem)levelListBox.SelectedItem;
                try
                {
                    IEditableCollectionView items = levelListBox.Items;
                    items.Remove(selectedLevel);
                    File.Delete(selectedLevel.Path);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LevelItem_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            // Assuming your PhotoItem class has a property called "Title"
            if (levelListBox.SelectedItem is LevelItem levelItem)
            {
                string clickedItemREADME = levelItem.Path + "/README";
                try
                {
                    string text = File.ReadAllText(clickedItemREADME);
                    text = ModifyLinks(text, levelItem.Path);
                    readmeTextBlock.Markdown = text;
                } catch (Exception ex)
                {
                    readmeTextBlock.Markdown = "No Readme";
                }
                if (levelItem.Path != null)
                    LoadPanel.Visibility = Visibility.Visible;
                else
                    LoadPanel.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateLevelList()
        {
            string levelsDirectory = Properties.Settings.Default.LevelsDirectory;

            try
            {
                string[] subDirectories = Directory.GetDirectories(levelsDirectory);

                List <LevelItem> levelItems = new List<LevelItem>();

                foreach (string subDirectory in subDirectories)
                {
                    if (File.Exists(subDirectory+"/SaveInfo.txt"))
                        levelItems.Add(new LevelItem(subDirectory));
                }

                if (levelItems.Count == 0)
                {
                    levelItems.Add(new LevelItem("No levels available", "Please add levels to the list."));
                }

                levelListBox.ItemsSource = levelItems;
            }
            catch (Exception ex)
            {
                List<LevelItem> levelItems = new List<LevelItem>();
                levelItems.Add(new LevelItem("No levels available", "Please add levels to the list."));

                levelListBox.ItemsSource = levelItems;
            }
        }
    }
}
