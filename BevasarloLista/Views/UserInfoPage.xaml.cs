using Xamarin.Forms;
using BevasarloLista.Resources;

namespace BevasarloLista.Views
{
    public partial class UserInfoPage : ContentPage
    {
        public UserInfoPage()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            nincknameLabel.Text = AppResource.nicknameLabel;
            userinfoSave.Text = AppResource.userinfoSave;
            base.OnAppearing();
        }
    }
}
