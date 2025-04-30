using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MyCollections
{
    public class CollectionItem
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Status { get; set; } = "nowy";
        public int Rating { get; set; } = 1;
        public string Comment { get; set; } = "";
    }

    public class Collection
    {
        public string Name { get; set; } = "";
        public List<CollectionItem> Items { get; set; } = new();
    }

    public static class FileService
    {
        private static string AppFolder =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyCollections");

        public static string GetDebugPath()
        {
            Debug.WriteLine($"Ścieżka do danych: {AppFolder}");
            return AppFolder;
        }

        public static List<Collection> LoadCollections()
        {
            var collections = new List<Collection>();
            Directory.CreateDirectory(AppFolder);

            foreach (var file in Directory.GetFiles(AppFolder, "*.txt"))
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var lines = File.ReadAllLines(file);
                var col = new Collection { Name = name };

                foreach (var line in lines)
                {
                    var p = line.Split('|');
                    if (p.Length < 5) continue;
                    col.Items.Add(new CollectionItem
                    {
                        Name = p[0],
                        Price = decimal.TryParse(p[1], out var pr) ? pr : 0,
                        Status = p[2],
                        Rating = int.TryParse(p[3], out var r) ? r : 1,
                        Comment = p[4]
                    });
                }

                collections.Add(col);
            }
            return collections;
        }

        public static void SaveCollection(Collection col)
        {
            Directory.CreateDirectory(AppFolder);
            var file = Path.Combine(AppFolder, $"{col.Name}.txt");
            var lines = col.Items.Select(i =>
                $"{i.Name}|{i.Price}|{i.Status}|{i.Rating}|{i.Comment}");
            File.WriteAllLines(file, lines);
        }

        public static void DeleteCollection(string name)
        {
            var file = Path.Combine(AppFolder, $"{name}.txt");
            if (File.Exists(file)) File.Delete(file);
        }
    }
}
