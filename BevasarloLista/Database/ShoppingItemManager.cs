#define OFFLINE_SYNC_ENABLED

using BevasarloLista.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
//using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif


namespace BevasarloLista.Database
{

    public class ShoppingItemManager
    {
        int i = 0;
        static ShoppingItemManager defaultInstance = new ShoppingItemManager();
     //   public IMobileServiceTable<ShoppingItem> ShoppingItemTable;
        public MobileServiceClient client;

        #if OFFLINE_SYNC_ENABLED
                IMobileServiceSyncTable<ShoppingItem> shoppingItemTable;
        #else
                        IMobileServiceTable<ShoppingItem> shoppingItemTable;
        #endif

        const string offlineDbPath = "ShoppingSQLite.db";

        private ShoppingItemManager()
        {
            this.client = new MobileServiceClient("https://bevasarlolista.azurewebsites.net");

        #if OFFLINE_SYNC_ENABLED
                    var store = new MobileServiceSQLiteStore(offlineDbPath);
                    store.DefineTable<ShoppingItem>();

                    //Initializes the SyncContext using the default IMobileServiceSyncHandler.
                    this.client.SyncContext.InitializeAsync(store);

                    this.shoppingItemTable = client.GetSyncTable<ShoppingItem>();
#else
                    this.shoppingItemTable = client.GetTable<ShoppingItem>();
#endif

        }


        public static ShoppingItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return shoppingItemTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<ShoppingItem>; }
        }

        public async Task<ObservableCollection<ShoppingItem>> GetShoppingItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<ShoppingItem> items = await shoppingItemTable
                   .IncludeTotalCount()
                    .ToEnumerableAsync();

                return new ObservableCollection<ShoppingItem>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(ShoppingItem item)
        {
            if (item.Id == null)
            {
                await shoppingItemTable.InsertAsync(item);
            }
            else
            {
                await shoppingItemTable.UpdateAsync(item);
            }
        }

        public async Task DeleteTaskAsync(ShoppingItem item) {

            await shoppingItemTable.DeleteAsync(item);

        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {

                await this.client.SyncContext.PushAsync();
                await this.shoppingItemTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "ShoppingItem",
                    this.shoppingItemTable.CreateQuery());

                //var task = this.client.SyncContext.PushAsync();
                //    await Task.WhenAny(task, Task.Delay(5000));
                //    var task2 = this.shoppingItemTable.PullAsync(
                //        //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //        //Use a different query name for each unique query in your program
                //        "ShoppingItem",
                //        this.shoppingItemTable.CreateQuery());
                //    await Task.WhenAny(task2, Task.Delay(5000));
               
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(@"Exception: {0}", ex.Message);
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}

