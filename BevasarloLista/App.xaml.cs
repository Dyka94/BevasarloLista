using Prism;
using Prism.Ioc;
using BevasarloLista.ViewModels;
using BevasarloLista.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;
using BevasarloLista.Services;
using BevasarloLista.Database;
using BevasarloLista.Auth;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BevasarloLista
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        // static ShoppingItemDatabase database;
        bool authenticated =false;
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
           
            await NavigationService.NavigateAsync("NavigationPage/LoginPage"); 
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<EditPage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<UserInfoPage>();

        }

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }



        //public static ShoppingItemDatabase Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            database = new ShoppingItemDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("ShoppingSQLite.db3"));
        //        }
        //        return database;
        //    }
        //}


    }
}
