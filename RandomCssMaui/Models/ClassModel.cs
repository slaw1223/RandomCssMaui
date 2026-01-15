using System.Collections.ObjectModel;

namespace RandomCssMaui.Models;

public class ClassModel
{
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<StudentModel> Students { get; set; } = new ObservableCollection<StudentModel>();

    public ClassModel() { }
    public ClassModel(string name) => Name = name;
}