using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Game/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public InventoryItem Item1, Item2, Outcome;
    }
}
