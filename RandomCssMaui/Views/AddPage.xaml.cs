using RandomCssMaui.ViewModels;

namespace RandomCssMaui.Views;

public partial class AddPage : ContentPage
{
    public AddPage()
    {
        InitializeComponent();
        var vm = new AddPageViewModel();
        BindingContext = vm;

        // dodaj przycisk w pasku narzêdzi do usuniêcia wszystkich klas i uczniów
        ToolbarItems.Add(new ToolbarItem
        {
            Text = "Usuñ wszystkie",
            Command = vm.RemoveAllCommand,
            Order = ToolbarItemOrder.Primary,
            Priority = 0
        });
    }
}