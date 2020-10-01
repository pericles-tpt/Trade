using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts.Core.Data
{

    public class ItemDataLoader : MonoBehaviour
    {
        string path = @".\goods.json";
        public Item[] Items;

        private void Awake()
        {
            Items = LoadItemData();
        }

        public Item[] LoadItemData()
        {
            // JSON
            string jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Item[]>(@jsonString);
        }

        public Item[] GetItems()
        {
            return Items;
        }

        public string GetItemName(int index)
        {
            return Items[index].name;
        }

        public int[] GetItemComponents(int index)
        {
            return Items[index].components;
        }

        public string GetItemCategory(int index)
        {
            return Items[index].category;
        }

        public int[] GetItemFound(int index)
        {
            return Items[index].found;
        }

        public int GetItemMass(int index)
        {
            return Items[index].mass;
        }

        public int GetItemBasePrice(int index)
        {
            return Items[index].bprice;
        }

        public int GetNoItems()
        {
            return Items.Length;
        }

        public class Item
        {
            public int index;
            public string name;
            public int[] components;
            public string category;
            public int[] found;
            public int mass;
            public int bprice;
        }

    }
}