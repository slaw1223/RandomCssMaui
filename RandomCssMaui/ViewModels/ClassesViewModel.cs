using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using RandomCssMaui.Models;
using RandomCssMaui.Data;

namespace RandomCssMaui.ViewModels;

public partial class ClassesViewModel : ObservableObject
{
    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    public ClassesViewModel()
    {
        _ = ClassRepository.LoadAsync();
    }
}