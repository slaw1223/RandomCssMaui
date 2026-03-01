using CommunityToolkit.Mvvm.ComponentModel;

namespace RandomCssMaui.Models;

public partial class StudentModel : ObservableObject
{
    [ObservableProperty]
    int id;

    [ObservableProperty]
    string name = string.Empty;

    [ObservableProperty]
    bool isPresent = true;

    [ObservableProperty]
    int selectedCounter;
}