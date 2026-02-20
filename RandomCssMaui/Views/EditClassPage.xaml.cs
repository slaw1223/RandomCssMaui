using System;
using RandomCssMaui.ViewModels;

namespace RandomCssMaui.Views;

public partial class EditClassPage : ContentPage
{
	public EditClassPage()
	{
		InitializeComponent();
        BindingContext = new EditClassViewModel();
    }
}