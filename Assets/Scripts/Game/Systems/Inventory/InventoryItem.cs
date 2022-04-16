using System;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
    public class InventoryItem : ScriptableObject
    {
        public Texture2D Icon;
        public GameObject Prefab;
        public ItemCategories Category;
        public string Name;
        public string Tooltip;

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        
        
    }
}