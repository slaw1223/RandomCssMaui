using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using RandomCssMaui.Models;
using RandomCssMaui.Data;

namespace RandomCssMaui.ViewModels;

public partial class AddPageViewModel : ObservableObject
{
    [ObservableProperty]
    string newClassName = string.Empty;

    [ObservableProperty]
    string newStudentName = string.Empty;

    [ObservableProperty]
    ClassModel? selectedClass;

    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    public AddPageViewModel()
    {
        _ = ClassRepository.LoadAsync();
    }

    [RelayCommand]
    void AddClass()
    {
        if (string.IsNullOrWhiteSpace(NewClassName))
            return;

        if (!Classes.Any(c => c.Name.Equals(NewClassName.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            Classes.Add(new ClassModel(NewClassName.Trim()));
            NewClassName = string.Empty;
        }
    }

    [RelayCommand]
    async Task AddStudent()
    {
        if (SelectedClass == null || string.IsNullOrWhiteSpace(NewStudentName))
            return;

        SelectedClass.AddStudent(NewStudentName.Trim());
        NewStudentName = string.Empty;
        await ClassRepository.SaveAsync();
    }

    [RelayCommand]
    async Task RemoveAll()
    {
        Classes.Clear();
        await ClassRepository.SaveAsync();
    }
}