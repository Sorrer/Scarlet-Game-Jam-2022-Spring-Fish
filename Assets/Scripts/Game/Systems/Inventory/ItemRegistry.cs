using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [Obsolete("Not used anymore")]
    public class ItemRegistry : ScriptableObject
    {
        [Serializable]
        public struct ItemSettings
        {
            public string id;
        }

        [SerializeField]
        private List<ItemSettings> items = new List<ItemSettings>();

        private Dictionary<string, ItemSettings> internalHash = new Dictionary<string, ItemSettings>();
        private void GenerateDatabase()
        {
            internalHash = new Dictionary<string, ItemSettings>();
            
            for(int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                
                item.id = Clean(item.id);
                //Replace with cleaned values
                items[i] = item;
                
                if (internalHash.ContainsKey(item.id))
                {
                    Debug.LogError($"Invalid Key {item.id} - Matches {internalHash[item.id]}, skipping object");
                }
                
                internalHash.Add(item.id, item);
            }
        }

        private string Clean(string id)
        {
            return Regex.Replace(id.ToLower(),@"[^0-9a-zA-Z_]+", "");
        }
        private void OnValidate()
        {
            GenerateDatabase();
        }

        public ItemSettings Get(string item)
        {
            if (!Clean(item).Equals(item))
            {
                Debug.LogWarning("Passing non-cleaned item, will not matched the database. Cleaning anyways.");
                item = Clean(item);
            }
            
            return internalHash[item];
        }

        
    }
}