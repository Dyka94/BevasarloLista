using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BevasarloLista.Models
{
    public class ShoppingItem
    {
        // [PrimaryKey, AutoIncrement]
        // public int Id { get; set; }
        //  [JsonProperty(PropertyName = "id")]
        public string NickName { get; set; }


        public string Id { get; set; }

      //  [JsonProperty(PropertyName = "item")]
        public string Item { get; set; }

      //  [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }

      //  [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


        //public ShoppingItem(int Id, string item, double amount, string type)
        //{
        //    this.Id = Id;
        //    this.Item = item;
        //   this.Amount = amount;
        //    this.Type = type;
        //}
        public ShoppingItem() { }
    }
}
