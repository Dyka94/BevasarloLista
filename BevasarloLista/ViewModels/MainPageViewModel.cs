using BevasarloLista.Database;
using BevasarloLista.Models;
using BevasarloLista.Resources;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace BevasarloLista.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        
        public ICommand PressCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LogoutCommand { get; }
        private bool isRefreshEnded;
        public bool IsRefreshEnded
        {
            get { return isRefreshEnded; }
            set { SetProperty(ref isRefreshEnded, value); }
        }
        private string nickName;
        public string NinckName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }

        ShoppingItemManager manager;




        private ObservableCollection<ShoppingItem> shoppingItems;
        private bool isConnectedToInternet;

        public ObservableCollection<ShoppingItem> ShoppingItems
        {
            get { return  shoppingItems; }
            set { SetProperty(ref shoppingItems, value);
                RaisePropertyChanged();
            }
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if ((string)parameters[NAVPARAM_NICKNAME] != null)
            {
                Application.Current.Properties["ninckname"] = (string)parameters[NAVPARAM_NICKNAME];
                await Application.Current.SavePropertiesAsync();
            }

            List<ShoppingItem> tmp = new List<ShoppingItem>();

            // tmp = await App.Database.GetItemsAsync();
            // ShoppingItems = tmp;
            ShoppingItems = await manager.GetShoppingItemsAsync(isConnectedToInternet); //ezt
                                                                                        //await manager.SyncAsync();

            try
            {
                var name = Application.Current.Properties["ninckname"].ToString();
            }
            catch (Exception ex)
            {
                await NavigationService.NavigateAsync("UserInfoPage");
            }

            base.OnNavigatedTo(parameters);
            
    }

        public MainPageViewModel(INavigationService navigationService ) 
            : base (navigationService)
        {
            this.DeleteCommand = new Command(onDelete);
            this.PressCommand = new Command(onPressed);
            this.EditCommand = new Command(onEdit);
            this.RefreshCommand = new Command(onRefresh);
            this.LogoutCommand=new Command(onLogout);
            manager = ShoppingItemManager.DefaultManager;
            // initList();

            Title = AppResource.mainTitle; //"Bevásárló Lista";
            

            if (CrossConnectivity.Current.IsConnected) isConnectedToInternet = true;
            else isConnectedToInternet = false;

            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (args.IsConnected)
                {
                    await RefreshItems(true);
                    isConnectedToInternet = true;
                }
                else {
                    isConnectedToInternet = false;
                }
            };
           
             

        }

        private async void onLogout(object obj)
        {
            var loggedOut = false;
            if (App.Authenticator != null) {
                loggedOut = await App.Authenticator.LogoutAsync();
            }
            if (loggedOut)
            {
                Application.Current.Properties[PARAM_EXPDATE] = null;
                Application.Current.Properties[NAVPARAM_NICKNAME] = null;
                await Application.Current.SavePropertiesAsync();

                await NavigationService.GoBackToRootAsync();
            }
            else {
                Console.WriteLine("Kijelentkezés nem sikerült");
            }
        }

        private async void initList()
        {
            //var savedList = await App.Database.GetItemsAsync();
            //if (savedList != null) ShoppingItems = savedList;
            //else ShoppingItems = new List<ShoppingItem>();
            ShoppingItems = await manager.GetShoppingItemsAsync(true);

        }

        private async void onEdit(object param)
        {
            //  ShoppingItem selectedItem = await App.Database.GetItemAsync((int)paramID); //ShoppingItems[(int)paramID];
            //var np = new NavigationParameters { { NAVPARAM_MODIFIEDITEM, selectedItem } };
            //await NavigationService.NavigateAsync("EditPage",np);
            await manager.SaveTaskAsync(param as ShoppingItem);
            var np = new NavigationParameters { { NAVPARAM_MODIFIEDITEM, param as ShoppingItem } };
            await NavigationService.NavigateAsync("EditPage",np);
        }

        private async void onDelete(object param)
        {

            //ShoppingItem deleteItem = await App.Database.GetItemAsync((int)paramId);
            //await App.Database.DeleteItemAsync(deleteItem);
            //ShoppingItems = await App.Database.GetItemsAsync();
          
            
            await manager.DeleteTaskAsync(param as ShoppingItem);
            await RefreshItems(isConnectedToInternet); //ezt

        }

        private async void onPressed(object obj)
        {
            var np = new NavigationParameters { { NAVPARAM_SHOPPINGITEM, newItemConst } };
            await NavigationService.NavigateAsync("EditPage", np);
        }

        //-------------------------------------------

        private async Task RefreshItems( bool syncItems)
        {
            
                ShoppingItems = await manager.GetShoppingItemsAsync(syncItems);
           
        }

        

        private async void onRefresh(object obj)
        {
            
            Exception error = null;
            try
            {
                    await RefreshItems(isConnectedToInternet);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                IsRefreshEnded = false;
                
              
            }

           
        }

    }
}
