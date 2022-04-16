using System;
using System.Collections.Generic;
using Game.Systems.Environment;
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

        public override string ToString()
        {
            return $"{name} - {Tooltip}";

        }
    
        [Header("Building Settings")]
        public List<DynamicForest.ProgressionEmersion> forestSettings = new List<DynamicForest.ProgressionEmersion>();
        // TODO Create an asset for events and it will call it
        public bool IsEndGame;
        
    }
}