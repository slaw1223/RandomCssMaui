using System.Threading;

namespace RandomCssMaui.Models;

public class StudentModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // flaga obecnoœci - domyœlnie true (obecny)
    public bool IsPresent { get; set; } = true;

    // Konstruktor bezparametrowy (deserializacja / XAML)
    public StudentModel() { }

    // Konstruktor pomocniczy (u¿ywany przed przypisaniem id)
    public StudentModel(string name)
    {
        Id = 0;
        Name = name;
        IsPresent = true;
    }

    // Konstruktor u¿ywany gdy id i obecnoœæ s¹ znane (np. przy wczytywaniu z pliku)
    public StudentModel(int id, string name, bool isPresent = true)
    {
        Id = id;
        Name = name;
        IsPresent = isPresent;
    }
}