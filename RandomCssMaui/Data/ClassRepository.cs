using System.Collections.ObjectModel;
using RandomCssMaui.Models;

namespace RandomCssMaui.Data;

public static class ClassRepository
{
    const string FileName = "classes.txt";
    static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    public static ObservableCollection<ClassModel> Classes { get; } = new ObservableCollection<ClassModel>();

    public static async Task LoadAsync()
    {
        Classes.Clear();
        if (!File.Exists(FilePath))
            return;

        var lines = await File.ReadAllLinesAsync(FilePath);
        ClassModel? current = null;

        foreach (var raw in lines)
        {
            var line = raw?.Trim();
            if (string.IsNullOrEmpty(line))
            {
                current = null;
                continue;
            }

            if (line.StartsWith("Class:"))
            {
                var name = line.Substring("Class:".Length).Trim();
                current = new ClassModel(name);
                Classes.Add(current);
            }
            else if (line.StartsWith("-") && current != null)
            {
                var studentRaw = line.Substring(1).Trim();
                // Format zapisu: "id|Name" (np. "- 12|Jan Kowalski")
                var parts = studentRaw.Split('|', 2);
                if (parts.Length == 2 && int.TryParse(parts[0], out var id))
                {
                    var studentName = parts[1].Trim();
                    current.Students.Add(new StudentModel(id, studentName));
                }
                else
                {
                    // stary format: tylko nazwa -> nadajemy id automatycznie
                    current.Students.Add(new StudentModel(studentRaw));
                }
            }
        }
    }

    public static async Task SaveAsync()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var cls in Classes)
        {
            sb.AppendLine($"Class: {cls.Name}");
            foreach (var s in cls.Students)
                // zapisujemy id wraz z nazw¹, separowane '|'
                sb.AppendLine($"- {s.Id}|{s.Name}");
            sb.AppendLine();
        }

        // upewnij siê, ¿e katalog istnieje przed zapisem
        var dir = Path.GetDirectoryName(FilePath) ?? FileSystem.AppDataDirectory;
        Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(FilePath, sb.ToString());
    }
}