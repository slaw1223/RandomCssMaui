using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using RandomCssMaui.Models;
using RandomCssMaui.Data;
using System.Threading.Tasks;

namespace RandomCssMaui.ViewModels;
public partial class AddPageViewModel : ObservableObject
{
    [ObservableProperty]
    ClassModel newClass = new ClassModel();

    [ObservableProperty]
    StudentModel newStudent = new StudentModel();

    [ObservableProperty]
    ClassModel selectedClass;

    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;
//pobiera kolekcjê z ClassRepository

    public AddPageViewModel()
    {
        _ = ClassRepository.LoadAsync();
//metoda jest async ale nie czeekamy na wynik 'discard'
    }

    [RelayCommand]
    async Task AddClass()
    {
        if (string.IsNullOrWhiteSpace(NewClass.NewClassName))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Nazwa klasy nie mo¿e byæ pusta", "OK");
            return;
        }

        if (!Classes.Any(c => c.Name.Equals(NewClass.NewClassName.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            Classes.Add(new ClassModel { Name = NewClass.NewClassName.Trim() });
            NewClass.NewClassName = string.Empty;
            await ClassRepository.SaveAsync();
        }
    }

    [RelayCommand]
    async Task AddStudent()
    {
        if (SelectedClass == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Wybierz klasê", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(NewStudent.NewStudentName))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Imiê ucznia nie mo¿e byæ puste", "OK");
            return;
        }

        ClassRepository.AddStudentToClass(SelectedClass, NewStudent.NewStudentName.Trim());
        NewStudent.NewStudentName = string.Empty;
        await ClassRepository.SaveAsync();
    }

    [RelayCommand]
    async Task RemoveAll()
    {
        await ClassRepository.ClearAllAsync();
    }
}