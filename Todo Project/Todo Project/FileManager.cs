using System;
using System.IO;


namespace Todo_Project
{
    internal class FileManager
    {
        public static void AddLineToFile(string path, string lineContent)
        {
            try
            {
                File.AppendAllText(path, lineContent + Environment.NewLine);
            }
            catch 
            {
                Console.WriteLine("Error");
            }
        }
        public static void RemoveLineAtIndex(string path, int index)
        {
            if (!File.Exists(path))
            {
                return;
            }
            var allLines = File.ReadAllLines(path);
            if (index < 0 || index > allLines.Length)
            {
                return;
            }
            var lines = new string[allLines.Length - 1];
            for (int i = 0, j = 0; i < allLines.Length; i++) 
            {
                if (i != index)
                {
                    lines[j++] = allLines[i];
                }
            }
            File.WriteAllLines(path, lines);
        }
        public static void UpdateLineAtIndex(string path, int index, string newContent)
        {
            if (!File.Exists (path))
            {
                return;
            }
            var allLines = File.ReadAllLines(path);
            allLines[index] = newContent;
            File.WriteAllLines(path, allLines);
        }
        public static void AddLineAtIndex(string path, int index, string newContent)
        {
            if (!File.Exists(path))
                return;
            var allLines = File.ReadAllLines(path);
            if (index < 0 || index > allLines.Length)
                return;
            var updatedLines = new string[allLines.Length + 1];
            for (int i = 0, j = 0; i < updatedLines.Length; i++)
            {
                if (i == index)
                {
                    updatedLines[i] = newContent;
                }
                else
                {
                    updatedLines[i] = allLines[j++];
                }
            }
            File.WriteAllLines(path, updatedLines);
        }
    }
}
