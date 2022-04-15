using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [CreateAssetMenu(fileName = "Crafting List", menuName = "Game/Crafting List")]
    public class CraftingList : ScriptableObject
    {

        public List<CraftingRecipe> preloadRecipes  = new List<CraftingRecipe>();
        
        private Dictionary<InventoryItem, Dictionary<InventoryItem, InventoryItem>> recipes = new Dictionary<InventoryItem,  Dictionary<InventoryItem, InventoryItem>>();

        public void Init()
        {
            foreach (var recipe in preloadRecipes)
            {
                if (CheckRecipe(recipe.Item1, recipe.Item2) != null)
                {
                    Debug.LogError("Found repeating recipe base, skipping. Take a look outcomes may differ");
                    continue;
                }

                if (recipes.TryGetValue(recipe.Item1, out var itemRecipes))
                {
                    itemRecipes.Add(recipe.Item2, recipe.Outcome);
                }
                else
                {
                    recipes.Add(recipe.Item1, new Dictionary<InventoryItem, InventoryItem>());
                    recipes[recipe.Item1].Add(recipe.Item2, recipe.Outcome);
                }
            }
        }

        private void OnValidate()
        {
            Init();
        }

        private void Awake()
        {
            Init();
        }

        public InventoryItem CheckRecipe(InventoryItem item1, InventoryItem item2)
        {
            if (recipes.ContainsKey(item1))
            {
                if (recipes[item1].ContainsKey(item2))
                {
                    return recipes[item1][item2];
                }
            }
            else if (recipes.ContainsKey(item2))
            {
                if (recipes[item2].ContainsKey(item1))
                {
                    return recipes[item2][item1];
                }
            }
            
            return null;
        }
    }
}

