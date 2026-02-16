using System.Collections.ObjectModel;
using System.Diagnostics;
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
                var parts = studentRaw.Split('|');
                if (parts.Length >= 2 && int.TryParse(parts[0], out var id))
                {
                    var studentName = parts[1].Trim();
                    bool isPresent = true;
                    var selectedCounter = 0;
                    if (parts.Length >= 3)
                    {
                        var p = parts[2].Trim();
                        if (int.TryParse(p, out var pInt))
                            isPresent = pInt != 0;
                        //else if (bool.TryParse(p, out var pBool))
                            //isPresent = pBool;
                        p = parts[3].Trim();
                        int.TryParse(p, out selectedCounter);
                    }

                    current.AddExistingStudent(new StudentModel(id, studentName, isPresent, selectedCounter));
                }
                else
                {
                    current.AddStudent(studentRaw);
                }
            }
        }
    }

    public static async Task SaveAsync()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var c in Classes)
        {
            sb.AppendLine($"Class: {c.Name}");
            foreach (var s in c.Students)
//1 obecny
                sb.AppendLine($"- {s.Id}|{s.Name}|{(s.IsPresent ? 1 : 0)}|{s.SelectedCounter}");
            sb.AppendLine();
        }

        var dir = Path.GetDirectoryName(FilePath) ?? FileSystem.AppDataDirectory;
        Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(FilePath, sb.ToString());
    }
}