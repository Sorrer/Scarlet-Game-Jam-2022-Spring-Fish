using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class CraftingManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private List<InventoryItemSlotController> inputSlots;
        [SerializeField]
        private InventoryItemSlotController outcomeSlot;
        [SerializeField]
        private List<CraftingRecipe> craftingRecipes;
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private InventorySO inventorySO;
        [SerializeField]
        private GameObject consumeItemFXPrefab;
        [SerializeField]
        private GameObject createItemFXPrefab;

        private void Awake()
        {
            foreach (var ingredientSlot in inputSlots)
                ingredientSlot.OnItemChanged.AddListener(TryCraft);
        }

        private void TryCraft()
        {
            // Cannot craft if there is an item in the way.
            if (outcomeSlot.ItemController != null)
                return;

            List<InventoryItem> inputItems = new List<InventoryItem>();
            foreach (var slot in inputSlots)
                if (slot.ItemController != null)
                    inputItems.Add(slot.ItemController.ItemData);

            foreach (var recipe in craftingRecipes)
            {
                // Definitely not craftable if we don't have the same input item count.
                if (recipe.Inputs.Length != inputItems.Count)
                    continue;

                var tempInputItems = new List<InventoryItem>(inputItems);
                foreach (var input in recipe.Inputs)
                    tempInputItems.Remove(input);

                if (tempInputItems.Count == 0 && !inventorySO.HasItem(recipe.Outcome))
                {
                    // Crafting is successful, we've used up all the items in the recipe.
                    // This item is also unique too -- otherwise we cannot craft it.
                    var outcomeItemController = Instantiate(itemPrefab, outcomeSlot.transform).GetComponent<InventoryItemController>();
                    outcomeSlot.ItemController = outcomeItemController;
                    Instantiate(createItemFXPrefab, outcomeSlot.transform.position, Quaternion.identity, outcomeSlot.transform);

                    inventorySO.AddItem(recipe.Outcome);

                    foreach (var slot in inputSlots)
                        if (slot.ItemController != null)
                        {
                            Instantiate(consumeItemFXPrefab, slot.transform.position, Quaternion.identity, slot.transform);
                            Destroy(slot.ItemController);
                            slot.ItemController = null;
                        }
                    break;
                }
            }
        }
    }
}