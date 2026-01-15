using RandomCssMaui.ViewModels;

namespace RandomCssMaui.Views;

public partial class AddPage : ContentPage
{
	public AddPage()
	{
		InitializeComponent();
        BindingContext = new AddPageViewModel();
	}
}