using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;


namespace Gloomwood_Level_Manager
{
    public class LevelItem
    {
        public string Path { get; }
        public string? Photo { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public LevelItem(string folderPath)
        {
            // Read values from the file or perform any necessary processing
            // For demonstration purposes, let's assume the file contains lines in the format: "photo_path|title|description"
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //Title = desktopPath;
            //string folderPath = Path.Combine(desktopPath, relPath);

            Path = folderPath;

            Photo = folderPath + "/Screenshot.jpg";

            string filePath = folderPath + "/SaveInfo.txt";
            try
            {
                string jsonContent = File.ReadAllText(filePath);

                JObject json = JObject.Parse(jsonContent);
                Title = (string)json["name"];
                Description = (string)json["description"];
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public LevelItem(string name, string description)
        {
            Title = name;
            Description = description;
        }
    }

}
