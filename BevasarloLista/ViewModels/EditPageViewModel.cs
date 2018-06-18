using System;
using System.Collections.Generic;
using System.Text;
using BevasarloLista.Database;
using BevasarloLista.Models;
using Microsoft.WindowsAzure.MobileServices;
using Prism.Navigation;
using Xamarin.Forms;

namespace BevasarloLista.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        public Command SaveCommand { get; }

        private ShoppingItemManager manager;

        private string id;
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set
            { 
                SetProperty(ref itemName, value);
          
            }
        }

        private string itemAmount;
        public string ItemAmount
        {
            get { return itemAmount; }
            set { SetProperty(ref itemAmount, value);
            
            }
        }

        private string itemAmountType;
        public string ItemAmountType
        {
            get { return itemAmountType; }
            set { SetProperty(ref itemAmountType, value);
            
            }
        }

        //private int itemIndex;
        bool isNewItem;
        ShoppingItem modifiedItem;

        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.SaveCommand = new Command(onSaved);
            this.manager = ShoppingItemManager.DefaultManager;
            

        }
        

        private async void onSaved(object obj)
        {
            double.TryParse(itemAmount, out double amount);
            ShoppingItem item;
            if (isNewItem)
            {
                item = new ShoppingItem();
                try
                {
                    item.NickName= Application.Current.Properties["ninckname"].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            else item = modifiedItem;
            //if (itemIndex == -1) item = new ShoppingItem();
            // item = await App.Database.GetItemAsync(itemIndex); //new ShoppingItem();
            item.Item = ItemName;
            item.Amount = amount;
            item.Type = ItemAmountType;
            // await App.Database.SaveItemAsync(item);
            
            await manager.SaveTaskAsync(item);
           // await manager.SyncAsync();
            var np = new NavigationParameters { { NAVPARAM_MODIFIEDITEM, item } };
            await NavigationService.GoBackAsync(np);
        }

        

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if ((string)parameters[NAVPARAM_SHOPPINGITEM] == newItemConst)
            {
                Title = Resources.AppResource.addTitle;//"Termék hozzáadása";
                isNewItem = true;
                
            }
            else {
                Title = Resources.AppResource.modifyTitle;// "Modosítás";
                isNewItem = false;
                modifiedItem = (ShoppingItem)parameters[NAVPARAM_MODIFIEDITEM];
               // itemIndex = selectedItem.Id;
                ItemName = modifiedItem.Item;
                ItemAmount = (modifiedItem.Amount).ToString();
                ItemAmountType = modifiedItem.Type;
            }

            base.OnNavigatedTo(parameters);
        }

       
    }
}
