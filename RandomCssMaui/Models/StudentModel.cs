using System.Threading;

namespace RandomCssMaui.Models;

public class StudentModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool IsPresent { get; set; } = true;

    public int SelectedCounter { get; set; } =  0;

    public StudentModel() { }

//konstruktor przed id
    public StudentModel(string name)
    {
        Id = 0;
        Name = name;
        IsPresent = true;
        SelectedCounter = 0;
    }

//konstruktor gdy id i obecnoœæ
    public StudentModel(int id, string name, bool isPresent = true, int selectedCounter=0)
    {
        Id = id;
        Name = name;
        IsPresent = isPresent;
        SelectedCounter = selectedCounter;
    }
}