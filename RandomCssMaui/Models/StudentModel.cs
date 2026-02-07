using System.Threading;

namespace RandomCssMaui.Models;

public class StudentModel
{
    // statyczny licznik id (bezpieczny w¹tkowo)
    private static int _nextId = 0;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Domyœlny konstruktor (przydatny przy deserializacji/ORM/XAML)
    public StudentModel()
    {
        Id = Interlocked.Increment(ref _nextId);
    }

    // Konstruktor przy dodawaniu nowego ucznia w aplikacji
    public StudentModel(string name)
    {
        Id = Interlocked.Increment(ref _nextId);
        Name = name;
    }

    // Konstruktor u¿ywany przy wczytywaniu z pliku - zachowuje oryginalne id
    public StudentModel(int id, string name)
    {
        Id = id;
        Name = name;
        EnsureNextIdGreaterThan(id);
    }

    // Upewnij siê, ¿e nastêpne automatyczne id bêdzie > max istniej¹cego
    private static void EnsureNextIdGreaterThan(int id)
    {
        int current;
        while (true)
        {
            current = _nextId;
            var desired = id + 1;
            if (current >= desired)
                break;
            if (Interlocked.CompareExchange(ref _nextId, desired, current) == current)
                break;
        }
    }
}