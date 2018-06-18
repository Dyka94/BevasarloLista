using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BevasarloLista.Resources;

namespace BevasarloLista.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
		public EditPage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            itemName.Placeholder = AppResource.itemNameEntry;
            itemAmount.Placeholder = AppResource.itemAmountEntry;
            itemAmountTypePicker.Title = AppResource.itemAmountTypePicker;
            addButton.Text = AppResource.saveButton;
            base.OnAppearing();
        }
    }
}