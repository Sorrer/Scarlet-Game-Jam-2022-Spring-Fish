using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Game/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public InventoryItem[] Inputs;
        public InventoryItem Output;
    }
}
