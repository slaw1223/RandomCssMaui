using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace RandomCssMaui.Models;

public class ClassModel
{
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<StudentModel> Students { get; set; } = new ObservableCollection<StudentModel>();

    // licznik id specyficzny dla tej klasy
    private int _nextStudentId = 0;

    public ClassModel(string name) => Name = name;

    // Tworzy ucznia z id przydzielonym per-klasie i dodaje go do kolekcji
    public StudentModel AddStudent(string name)
    {
        // zainicjalizuj licznik na podstawie istniej¹cych uczniów przy pierwszym u¿yciu
        if (_nextStudentId == 0)
        {
            _nextStudentId = Students.Any() ? Students.Max(s => s.Id) : 0;
        }

        var id = Interlocked.Increment(ref _nextStudentId);
        var student = new StudentModel(id, name);
        Students.Add(student);
        return student;
    }

    // przy dodawaniu istniej¹cego ucznia (np. przy wczytywaniu) zapewnij w³aœciwy nextId
    public void AddExistingStudent(StudentModel student)
    {
        Students.Add(student);
        // ustaw licznik tak, ¿eby nastêpne id by³o wiêksze ni¿ istniej¹ce
        int observed = _nextStudentId;
        while (observed <= student.Id)
        {
            if (Interlocked.CompareExchange(ref _nextStudentId, student.Id, observed) == observed)
                break;
            observed = _nextStudentId;
        }
    }
}