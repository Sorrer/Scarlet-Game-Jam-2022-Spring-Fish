using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Systems.Inventory
{
    public enum ItemCategories
    {
        Buildings, Food, Craftable
    }
    
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "Game/Inventory")]
    public class InventorySO : ScriptableObject
    {
        //Only one of each item
        private HashSet<InventoryItem> items = new HashSet<InventoryItem>();
        
        [SerializeField]
        private InventoryItem[] preloadItems;

        public InventoryItem HeldItem;
        public HashSet<InventoryItem> Items => items;

        private void OnEnable()
        {
            Items.Clear();
            foreach (var item in preloadItems)
                Items.Add(item);
        }

        public List<InventoryItem> GetAllitems()
        {
            return Items.ToList();
        }

        public List<InventoryItem> GetSortedItems()
        {
            var sorted = new List<InventoryItem>();
            sorted.AddRange(GetAllitems(ItemCategories.Food));
            sorted.AddRange(GetAllitems(ItemCategories.Buildings));
            sorted.AddRange(GetAllitems(ItemCategories.Craftable));
            return sorted;
        }
        
        public List<InventoryItem> GetAllitems(ItemCategories category)
        {
            List<InventoryItem> sortedItems = new List<InventoryItem>();

            foreach (var i in Items)
            {
                if (i.Category == category)
                {
                    sortedItems.Add(i);
                }
            }

            return sortedItems;
        }
        

        public void AddItem(InventoryItem item)
        {
            if (Items.Contains(item))
            {
                Debug.LogWarning("Item already exists, can not add. Check first");
                return;
            }
            
            Items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
        }
        
        

        public bool HasItem(InventoryItem item)
        {
            return Items.Contains(item);
        }
    }
}
