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

        if (Application.Current is App app)
            luckyNumber = app.LuckyNumber;
        else
            luckyNumber = new Random().Next(1, 31);
    }

    [ObservableProperty]
    ClassModel? selectedClass;

    [ObservableProperty]
    string selectedStudentName = string.Empty;

    [ObservableProperty]
    int luckyNumber;


    [RelayCommand]
    async Task DrawStudent()
    {
        if (SelectedClass == null)
        {
            SelectedStudentName = "Wybierz klasê";
            return;
        }

        if (!SelectedClass.Students.Any(s => s.IsPresent))
        {
            SelectedStudentName = "Brak obecnych uczniów w klasie";
            return;
        }
        var candidates = SelectedClass.Students.Where(s => s.IsPresent && s.Id != LuckyNumber && s.SelectedCounter<=0).ToList();

        if (!candidates.Any())
        {
            SelectedStudentName = "Brak kandydata";
            return;
        }

        var rnd = new Random();
        var winner = candidates[rnd.Next(candidates.Count)];
        foreach(var s in SelectedClass.Students)
        {
            if(s.SelectedCounter > 0)
            s.SelectedCounter -= 1;
            if (s.Id == winner.Id)
                s.SelectedCounter = 3;
        }

        SelectedStudentName = $"{winner.Id} {winner.Name}";
        await ClassRepository.SaveAsync();
    }
}