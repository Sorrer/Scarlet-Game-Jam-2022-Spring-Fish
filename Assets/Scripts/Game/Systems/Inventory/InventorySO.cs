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

        private void Awake()
        {
            items.Clear();
            
        }

        public InventoryItem HeldItem;
    
        public List<InventoryItem> GetAllitems()
        {
            return items.ToList();
        }
        
        public List<InventoryItem> GetAllitems(ItemCategories category)
        {
            List<InventoryItem> sortedItems = new List<InventoryItem>();

            foreach (var i in items)
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
            if (items.Contains(item))
            {
                Debug.LogWarning("Item already exists, can not add. Check first");
                return;
            }
            
            items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            items.Remove(item);
        }
        
        

        public bool HasItem(InventoryItem item)
        {
            return items.Contains(item);
        }
    }
}
