using BevasarloLista.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BevasarloLista.Services
{
    public class ShoppingItemDatabase
    {

        readonly SQLiteAsyncConnection database;

        public ShoppingItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ShoppingItem>().Wait();
        }

        public Task<List<ShoppingItem>> GetItemsAsync()
        {
            return database.Table<ShoppingItem>().ToListAsync();
        }

      
       //Todo: ez uncommentelnmi
        //public Task<ShoppingItem> GetItemAsync(int id)
        //{
        //    return database.Table<ShoppingItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        //}

        //public Task<int> SaveItemAsync(ShoppingItem item)
        //{
        //    if (item.Id != 0)
        //    {
        //        return database.UpdateAsync(item);
        //    }
        //    else
        //    {
        //        return database.InsertAsync(item);
        //    }
        //}

        public Task<int> DeleteItemAsync(ShoppingItem item)
        {
            return database.DeleteAsync(item);
            
        }
        public Task<int> UpdateItemAsync(ShoppingItem item) {
            return database.UpdateAsync(item);

        }

        

    }
}
