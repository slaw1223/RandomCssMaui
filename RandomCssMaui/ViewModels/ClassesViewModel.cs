using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RandomCssMaui.Models;
using RandomCssMaui.Data;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace RandomCssMaui.ViewModels;

public partial class ClassesViewModel : ObservableObject
{
    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    public ClassesViewModel()
    {
        _ = ClassRepository.LoadAsync();
    }

    [ObservableProperty]
    ClassModel? selectedClass;

    [ObservableProperty]
    string selectedStudentName = string.Empty;

    [ObservableProperty]
    int luckyNumber;

    [RelayCommand]
    void DrawStudent()
    {
        if (SelectedClass == null)
        {
            SelectedStudentName = "Wybierz klasê";
            return;
        }

        if (!SelectedClass.Students.Any())
        {
            SelectedStudentName = "Brak uczniów w klasie";
            return;
        }

        // Pobierz szczêœliwy numerek wygenerowany przy starcie aplikacji
        if (Application.Current is App app)
            LuckyNumber = app.LuckyNumber;
        else
            LuckyNumber = new Random().Next(1, 31);

        // Wyklucz uczniów o id == luckyNumber
        var candidates = SelectedClass.Students.Where(s => s.Id != LuckyNumber).ToList();

        if (!candidates.Any())
        {
            SelectedStudentName = $"Wszyscy uczniowie maj¹ szczêœliwy numer {LuckyNumber} — brak kandydata";
            return;
        }

        var rnd = new Random();
        var winner = candidates[rnd.Next(candidates.Count)];
        SelectedStudentName = $"{winner.Name} (Id: {winner.Id})";
    }
}