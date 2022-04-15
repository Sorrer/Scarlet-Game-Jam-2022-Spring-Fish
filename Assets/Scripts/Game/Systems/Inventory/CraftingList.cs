using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [CreateAssetMenu(fileName = "Crafting List", menuName = "Game/Crafting List")]
    public class CraftingList : ScriptableObject
    {

        private Dictionary<InventoryItem, CraftingRecipe> recipes = new Dictionary<InventoryItem, CraftingRecipe>();

        public InventoryItem CheckRecipe(InventoryItem item1, InventoryItem item2)
        {
            if (recipes.ContainsKey(item1))
            {
                if (recipes[item1].Item2 == item2)
                {
                    return recipes[item1].Outcome;
                }
            }
            else if (recipes.ContainsKey(item2))
            {
                if (recipes[item2].Item1 == item1)
                {
                    return recipes[item2].Outcome;
                }
            }
            
            return null;
        }
    }
}

