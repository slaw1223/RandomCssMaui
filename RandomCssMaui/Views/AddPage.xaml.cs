using RandomCssMaui.ViewModels;

namespace RandomCssMaui.Views;

public partial class AddPage : ContentPage
{
    public AddPage()
    {
        InitializeComponent();
        var vm = new AddPageViewModel();
        BindingContext = vm;
    }
}