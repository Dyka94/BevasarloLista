using Xamarin.Forms;
using BevasarloLista.Resources;

namespace BevasarloLista.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            loginFBButton.Text = AppResource.loginFBButton;
            loginGoogleButton.Text = AppResource.loginGoogleButton;
            base.OnAppearing();
        }
    }
}
