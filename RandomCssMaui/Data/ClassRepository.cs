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
                // Nowy format zapisu: "id|Name|present" gdzie present = 1 lub 0
                var parts = studentRaw.Split('|');
                if (parts.Length >= 2 && int.TryParse(parts[0], out var id))
                {
                    var studentName = parts[1].Trim();
                    bool isPresent = true;
                    if (parts.Length >= 3)
                    {
                        var p = parts[2].Trim();
                        if (int.TryParse(p, out var pInt))
                            isPresent = pInt != 0;
                        else if (bool.TryParse(p, out var pBool))
                            isPresent = pBool;
                    }

                    current.AddExistingStudent(new StudentModel(id, studentName, isPresent));
                }
                else
                {
                    // stary format: tylko nazwa -> nadajemy id automatycznie i obecnoœæ = true
                    current.AddStudent(studentRaw);
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
                // zapisujemy id, nazwê i flagê obecnoœci (1 = obecny, 0 = nieobecny)
                sb.AppendLine($"- {s.Id}|{s.Name}|{(s.IsPresent ? 1 : 0)}");
            sb.AppendLine();
        }

        var dir = Path.GetDirectoryName(FilePath) ?? FileSystem.AppDataDirectory;
        Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(FilePath, sb.ToString());
    }
}