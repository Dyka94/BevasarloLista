using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BevasarloLista.Auth;
using Prism.Navigation;
using Xamarin.Forms;

namespace BevasarloLista.ViewModels
{
    class LoginPageViewModel : ViewModelBase
    {

        public Command LoginFacebookCommand { get; }
        public Command LoginGoogleCommand { get; }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = Resources.AppResource.loginTitle;// "Bejelentkezés";
            LoginFacebookCommand = new Command(onFBLogin);
            LoginGoogleCommand = new Command(onGoogleLogin);

          

        }
        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                var expdate = (DateTime)Application.Current.Properties[PARAM_EXPDATE];
                if (expdate >= DateTime.Today) await NavigationService.NavigateAsync("MainPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Nincs token mentve");

            }
            base.OnNavigatedTo(parameters);
        }


        private async void onGoogleLogin(object obj)
        {
            DateTime authenticated=new DateTime();
            if (App.Authenticator != null)
            {
                authenticated = await App.Authenticator.Authenticate(ProvidersEnum.Google);
            }
            if (!authenticated.Equals(new DateTime(1, 1, 1)))
            {
                try
                {
                    Application.Current.Properties[PARAM_EXPDATE] = authenticated;
                    await NavigationService.NavigateAsync("MainPage");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                
            }
        }

        private async void onFBLogin(object obj)
        {
            DateTime authenticated = new DateTime();
            if (App.Authenticator != null) {
                authenticated = await App.Authenticator.Authenticate(ProvidersEnum.Facebook);
            }
            if (!authenticated.Equals(new DateTime(1, 1, 1)))
            {
                try
                {
                    Application.Current.Properties[PARAM_EXPDATE] = authenticated;
                   await Application.Current.SavePropertiesAsync();
                    await NavigationService.NavigateAsync("MainPage");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
