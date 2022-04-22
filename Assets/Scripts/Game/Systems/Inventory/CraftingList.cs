using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory
{
    public class RecipeTreeNode
    {
        // Note that Output is the output gained from the entire chain of input nodes before it
        public InventoryItem Output { get; set; }
        public Dictionary<InventoryItem, RecipeTreeNode> Children { get; private set; } = new Dictionary<InventoryItem, RecipeTreeNode>();
    }

    [CreateAssetMenu(fileName = "Crafting List", menuName = "Game/Crafting List")]
    public class CraftingList : ScriptableObject
    {
        [SerializeField]
        private List<CraftingRecipe> recipes = new List<CraftingRecipe>();
        public List<CraftingRecipe> Recipes => recipes;

        private HashSet<InventoryItem> allItems = new HashSet<InventoryItem>();
        public HashSet<InventoryItem> AllItems => allItems;

        private Dictionary<InventoryItem, int> itemIndexes = new Dictionary<InventoryItem, int>();

        private int nextFreeItemIdx = 0;

        private RecipeTreeNode recipeTreeRoot;

        private List<InventoryItem> GetIndexSortedInputs(CraftingRecipe recipe)
        {
            return GetIndexSortedInputs(recipe.Inputs);
        }

        public List<InventoryItem> buildingsBuilt = new List<InventoryItem>();
        
        private void OnEnable()
        {
            buildingsBuilt.Clear();
        }

        // Returns inputs of recipe sorted by the index given by InventoryItemsManager
        private List<InventoryItem> GetIndexSortedInputs(IEnumerable<InventoryItem> inputs)
        {
            var sortedList = new List<InventoryItem>(inputs);
            // 0 is first, 1, 2, 3 are later
            sortedList.Sort((x, y) => GetItemIndex(x) - GetItemIndex(y));
            return sortedList;
        }

        public int GetItemIndex(InventoryItem item)
        {
            if (itemIndexes.TryGetValue(item, out int result))
                return result;
            return -1;
        }

        public void Init()
        {
            SetupAllItems();
            SetupItemIndices();
            SetupRecipeTree();
        }

        public bool TryCraft(IEnumerable<InventoryItem> inputs, out InventoryItem output)
        {
            output = null;
            var result = Craft(inputs);
            if (result == null)
                return false;
            output = result;
            return true;
        }

        public InventoryItem Craft(IEnumerable<InventoryItem> inputs)
        {
            var sortedInputsQueue = new Queue<InventoryItem>(GetIndexSortedInputs(inputs));
            var currNode = recipeTreeRoot;

            while (sortedInputsQueue.Count > 0)
            {
                var currInput = sortedInputsQueue.Dequeue();
                if (!currNode.Children.TryGetValue(currInput, out currNode))
                    // Recipe tree traversal failed, meaning we did not match a recipe
                    return null;
            }

            if (currNode.Output.Category == ItemCategories.Buildings)
            {
                if (buildingsBuilt.Contains(currNode.Output))
                {
                    return null;
                }
            }
            // We've travesed successfully to the output location
            // (Note that this is not necessarily the last node in a chain)
            return currNode.Output;
        }

        private void SetupRecipeTree()
        {
            // Note that the root does not have an InputItem
            recipeTreeRoot = new RecipeTreeNode();

            foreach (var recipe in recipes)
            {
                // Assuming all recipes are order independent
                var sortedInputsQueue = new Queue<InventoryItem>(GetIndexSortedInputs(recipe));
                var currNode = recipeTreeRoot;

                while (sortedInputsQueue.Count > 0)
                {
                    var currInput = sortedInputsQueue.Dequeue();
                    if (!currNode.Children.ContainsKey(currInput))
                    {
                        currNode.Children.Add(currInput, new RecipeTreeNode());
                    }

                    currNode = currNode.Children[currInput];
                }

                // Output already exists at this location, therefore we must have a duplicate recipe
                // that had set this Output to it's outcome.
                if (currNode.Output != null)
                    Debug.LogWarning($"Found duplicate recipe when parsing \"{recipe.name}\" recipe. Consider removing the duplicate");

                currNode.Output = recipe.Output;
            }
        }

        private void SetupAllItems()
        {
            AllItems.Clear();

            foreach (var recipe in recipes)
            {
                foreach (var input in recipe.Inputs)
                    AllItems.Add(input);
                AllItems.Add(recipe.Output);
            }
        }

        private void SetupItemIndices()
        {
            nextFreeItemIdx = 0;
            itemIndexes.Clear();

            foreach (var item in allItems)
                itemIndexes.Add(item, nextFreeItemIdx++);
        }

        private void OnValidate()
        {
            Init();
        }

        private void Awake()
        {
            Init();
        }
    }
}

