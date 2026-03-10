using System.Collections.ObjectModel;
using RandomCssMaui.Models;

namespace RandomCssMaui.Data;

public static class ClassRepository
{
    const string FileName = "classes.txt";
    static string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

    static readonly Dictionary<ClassModel, int> _nextIds = new();
    static readonly object _lock = new();
//zapobiego duplikacji _nextIds
    public static ObservableCollection<ClassModel> Classes { get; } = new ObservableCollection<ClassModel>();

    public static async Task LoadAsync()
    {
        Classes.Clear();
        lock (_lock)
            _nextIds.Clear();

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
                current = new ClassModel { Name = name };
                Classes.Add(current);
                lock (_lock)
                    _nextIds[current] = 0;
            }
            else if (line.StartsWith("-") && current != null)
            {
                var studentRaw = line.Substring(1).Trim();
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
                    }
                    int SelectedCounter = 0;
                    if (parts.Length >= 4)
                    {
                        var c = parts[3].Trim();
                        if (int.TryParse(c, out var cInt))
                        SelectedCounter = cInt;
                            
                    }

                    var student = new StudentModel { Id = id, Name = studentName, IsPresent = isPresent, SelectedCounter = SelectedCounter };
                    current.Students.Add(student);

                    lock (_lock)
                    {
                        if (!_nextIds.TryGetValue(current, out var cur)) cur = 0;
                        if (id > cur) _nextIds[current] = id;
                    }
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
                sb.AppendLine($"- {s.Id}|{s.Name}|{(s.IsPresent ? 1 : 0)}|{(s.SelectedCounter)}");
            sb.AppendLine();
        }

        var dir = Path.GetDirectoryName(FilePath) ?? FileSystem.AppDataDirectory;
        Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(FilePath, sb.ToString());
    }

    public static StudentModel AddStudentToClass(ClassModel cls, string name)
    {
        if (cls == null) throw new ArgumentNullException(nameof(cls));
        lock (_lock)
        {
            if (!_nextIds.TryGetValue(cls, out var last)) 
                last = cls.Students.Any() ? cls.Students.Max(s => s.Id) : 0;
            last++;
            _nextIds[cls] = last;

            var student = new StudentModel { Id = last, Name = name, IsPresent = true, SelectedCounter = 0 };
            cls.Students.Add(student);
            return student;
        }
    }
    public static async Task ClearAllAsync()
    {
        lock (_lock)
        {
            _nextIds.Clear();
        }
        Classes.Clear();
        await SaveAsync();
    }
}