using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using RandomCssMaui.Models;
using RandomCssMaui.Data;
using System.Threading.Tasks;

namespace RandomCssMaui.ViewModels;
//Pozbyæ siê zmiennych z viewmodeli -> wszystko do modeli
public partial class AddPageViewModel : ObservableObject
{
    [ObservableProperty]
    string newClassName = string.Empty;

    [ObservableProperty]
    string newStudentName = string.Empty;

    [ObservableProperty]
    ClassModel selectedClass;

    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    public AddPageViewModel()
    {
        _ = ClassRepository.LoadAsync();
    }

    [RelayCommand]
    async Task AddClass()
    {
        if (string.IsNullOrWhiteSpace(NewClassName))
        return;

        if (!Classes.Any(c => c.Name.Equals(NewClassName.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            Classes.Add(new ClassModel { Name = NewClassName.Trim() });
            NewClassName = string.Empty;
            await ClassRepository.SaveAsync();
        }
    }

    [RelayCommand]
    async Task AddStudent()
    {
        if (SelectedClass == null || string.IsNullOrWhiteSpace(NewStudentName))
            return;

        ClassRepository.AddStudentToClass(SelectedClass, NewStudentName.Trim());
        NewStudentName = string.Empty;
        await ClassRepository.SaveAsync();
    }

    [RelayCommand]
    async Task RemoveAll()
    {
        await ClassRepository.ClearAllAsync();
    }
}