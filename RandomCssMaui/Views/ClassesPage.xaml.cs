using RandomCssMaui.ViewModels;

namespace RandomCssMaui.Views;

public partial class ClassesPage : ContentPage
{
	public ClassesPage()
	{
		InitializeComponent();
		BindingContext = new ClassesViewModel();
	}
}