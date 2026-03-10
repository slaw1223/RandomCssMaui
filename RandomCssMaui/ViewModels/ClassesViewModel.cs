using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using RandomCssMaui.Models;
using RandomCssMaui.Data;
using System;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RandomCssMaui.ViewModels;

public partial class ClassesViewModel : ObservableObject
{
    SerialPort port;
    public ObservableCollection<ClassModel> Classes => ClassRepository.Classes;

    public ClassesViewModel()
    {
        luckyNumber = new Random().Next(1, 31);
    }

    [ObservableProperty]
    ClassModel? selectedClass;

    [ObservableProperty]
    StudentModel selectedStudentName = new StudentModel();

    [ObservableProperty]
    int luckyNumber;

    [ObservableProperty]
    string arduinoConnectionStatus = "Nie po³¹czono";


    [RelayCommand]
    void ConnectArduino()
    {
        try
        {
            port = new SerialPort("COM4", 9600);
            port.ReadTimeout = 500;
            port.Open();
            ArduinoConnectionStatus = "Po³¹czono";
        }
        catch (Exception ex)
        {
            ArduinoConnectionStatus = ex.Message;
            Console.WriteLine($"Error connecting to Arduino: {ex.Message}");
        }
    }

    [RelayCommand]
    async Task DrawStudent()
    {
        if (SelectedClass == null)
        {
            await App.Current.MainPage.DisplayAlert("Error", "Wybierz klasê", "OK");
            return;
        }

        if (!SelectedClass.Students.Any(s => s.IsPresent))
        {
            SelectedStudentName.DisplayName = "Brak obecnych uczniów w klasie";
            return;
        }
        var candidates = SelectedClass.Students.Where(s => s.IsPresent && s.Id != LuckyNumber && s.SelectedCounter<=0).ToList();

        if (!candidates.Any())
        {
            SelectedStudentName.DisplayName = "Brak kandydata";
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
        if(port != null && port.IsOpen)
            port.WriteLine($"START_ANIM | {winner.Id}");
        SelectedStudentName.DisplayName = $"{winner.Id} {winner.Name}";
        await ClassRepository.SaveAsync();
    }
    [RelayCommand]
    async Task SavePresency()
    {
        await ClassRepository.SaveAsync();
    }
}