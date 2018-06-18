using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BevasarloLista.Resources;

namespace BevasarloLista.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

		}
        protected override void OnAppearing()
        {
            addButton.Text = AppResource.addButton;
            signOutButton.Text = AppResource.signOutButton;
            
            base.OnAppearing();
           
        }
    }
}