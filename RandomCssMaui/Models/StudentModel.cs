namespace RandomCssMaui.Models;

public class StudentModel
{
    public string Name { get; set; } = string.Empty;
    //public StudentModel() { }
    public StudentModel(string name) => Name = name;
}