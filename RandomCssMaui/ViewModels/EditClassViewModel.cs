using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using RandomCssMaui.Models;
using RandomCssMaui.Data;
using System.Threading.Tasks;

namespace RandomCssMaui.ViewModels;

public partial class EditClassViewModel : ObservableObject
{
    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;
    [ObservableProperty]
    ClassModel? selectedClass;

    [ObservableProperty]
    StudentModel? selectedStudent;

    [ObservableProperty]
    StudentModel newStudent = new StudentModel();

    [ObservableProperty]
    bool isEditEnabled;

    public EditClassViewModel()
    {
    }

    public Task InitializeAsync() => ClassRepository.LoadAsync();

    [RelayCommand]
    async Task AddStudent()
    {
        if (SelectedClass == null) {
            await App.Current.MainPage.DisplayAlert("Error", "Wybierz klasę", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(NewStudent.NewStudentName))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Imię ucznia nie może być puste", "OK");
            return;
        }

        ClassRepository.AddStudentToClass(SelectedClass, NewStudent.NewStudentName.Trim());
        NewStudent.NewStudentName = string.Empty;
        await ClassRepository.SaveAsync();
    }

    [RelayCommand]
    async Task RemoveStudent(StudentModel? student)
    {
        var s = student ?? SelectedStudent;
        if (SelectedClass == null || s == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Wybierz klasę", "OK");
            return;
        }
            

        SelectedClass.Students.Remove(s);
        if (SelectedStudent == s)
            SelectedStudent = null;
        await ClassRepository.SaveAsync();
    }

    [RelayCommand]
    void BeginEditStudent(StudentModel? student)
    {
        var s = student ?? SelectedStudent;
        if (s == null)
            return;

        SelectedStudent = s;
        SelectedStudent.EditStudentName = s.Name;
        IsEditEnabled = true;
    }

    [RelayCommand]
    async Task SaveEditStudent()
    {
        if (SelectedStudent == null || SelectedClass == null || string.IsNullOrWhiteSpace(SelectedStudent.EditStudentName))
        {
            await App.Current.MainPage.DisplayAlert("Error", "Imię ucznia nie może być puste", "OK");
            return;
        }
        SelectedStudent.Name = SelectedStudent.EditStudentName.Trim();
        SelectedStudent.EditStudentName = string.Empty;
        IsEditEnabled = false;
        await ClassRepository.SaveAsync();
    }
}