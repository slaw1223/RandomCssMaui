using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RandomCssMaui.Models;
using RandomCssMaui.Data;

namespace RandomCssMaui.ViewModels;

public partial class EditClassViewModel : ObservableObject
{
    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    [ObservableProperty]
    ClassModel? selectedClass;

    [ObservableProperty]
    StudentModel? selectedStudent;

    [ObservableProperty]
    string newStudentName = string.Empty;

    [ObservableProperty]
    string editStudentName = string.Empty;

    public EditClassViewModel()
    {
    }

    public Task InitializeAsync() => ClassRepository.LoadAsync();

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
    async Task RemoveStudent(StudentModel? student)
    {
        var s = student ?? SelectedStudent;
        if (SelectedClass == null || s == null)
            return;

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
        EditStudentName = s.Name;
    }

    [RelayCommand]
    async Task SaveEditStudent()
    {
        if (SelectedStudent == null || SelectedClass == null || string.IsNullOrWhiteSpace(EditStudentName))
            return;

        // StudentModel implementuje powiadamianie — ustawienie Name odświeży UI
        SelectedStudent.Name = EditStudentName.Trim();
        EditStudentName = string.Empty;
        await ClassRepository.SaveAsync();
    }
}