using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using BevasarloLista.Auth;
using BevasarloLista.Database;
using System;
using System.IdentityModel.Tokens.Jwt;
using Android.Webkit;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Widget;

namespace BevasarloLista.Droid
{
    

    [Activity(Label = "BevasarloLista", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {

        private MobileServiceUser user;
        TextView msgText;

        public async Task<bool> LogoutAsync()
        {

             CookieManager.Instance.RemoveAllCookie();
            try
            {
                await ShoppingItemManager.DefaultManager.CurrentClient.LogoutAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            

}

        public async Task<DateTime> Authenticate(ProvidersEnum provider)
        {
            DateTime success = new DateTime();

             var message = string.Empty;
           
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (provider == ProvidersEnum.Facebook)
                {
                    // Sign in with Facebook login using a server-managed flow.
                    user = await ShoppingItemManager.DefaultManager.CurrentClient.LoginAsync(this,
                        MobileServiceAuthenticationProvider.Facebook, "bevasarlolista");
                    if (user != null)
                    {
                        var token = handler.ReadToken(user.MobileServiceAuthenticationToken);
                        success = token.ValidTo;
                     //   success = true;
                        
                       
                    }
                } else if (provider == ProvidersEnum.Google) {
                    user = await ShoppingItemManager.DefaultManager.CurrentClient.LoginAsync(this,
                       MobileServiceAuthenticationProvider.Google, "bevasarlolista");
                    if (user != null)
                    {
                        var token = handler.ReadToken(user.MobileServiceAuthenticationToken);
                        success = token.ValidTo;
                        //    success = true;

                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                Console.WriteLine(message);
                //// Display the  failure message.
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(message);
                builder.SetTitle("Sign-in result");
                builder.Create().Show();
            }

            

            return success;
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)) { }
                //     msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    //     msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
            //    msgText.Text = "Google Play Services is available.";
                return true;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {

            Console.WriteLine("GoogleApiAvailability szolgaltatas vane:"+ IsPlayServicesAvailable());
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            App.Init((IAuthenticate)this);


            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));

        }

        

    }

    

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            // Register any platform specific implementations
        }
    }
}

