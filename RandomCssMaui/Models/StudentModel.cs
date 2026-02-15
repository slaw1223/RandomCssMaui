using System.Threading;

namespace RandomCssMaui.Models;

public class StudentModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Konstruktor bezparametrowy (deserializacja / XAML)
    public StudentModel() { }

    // Konstruktor pomocniczy (mo¿e byæ u¿ywany przez ClassModel przy tworzeniu przed przypisaniem id)
    public StudentModel(string name)
    {
        Id = 0;
        Name = name;
    }

    // Konstruktor u¿ywany gdy id jest znane (np. przy wczytywaniu z pliku)
    public StudentModel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}