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
        private InventoryItemSlotController outputSlot;
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private GameObject consumeItemFXPrefab;
        [SerializeField]
        private GameObject createItemFXPrefab;
        [SerializeField]
        private InventorySO inventorySO;
        [SerializeField]
        private CraftingList craftingList;

        private void Awake()
        {
            foreach (var ingredientSlot in inputSlots)
                ingredientSlot.OnItemChanged.AddListener(TryCraft);
        }

        private void TryCraft()
        {
            // Cannot craft if there is an item in the way.
            if (outputSlot.ItemController != null)
                return;

            List<InventoryItem> inputItems = new List<InventoryItem>();
            foreach (var slot in inputSlots)
                if (slot.ItemController != null)
                    inputItems.Add(slot.ItemController.ItemData);

            if (craftingList.TryCraft(inputItems, out InventoryItem outputItem))
            {
                // We assume fx prefabs are one shot and clean up themselves
                foreach (var inputSlot in inputSlots)
                    Instantiate(consumeItemFXPrefab, inputSlot.transform.position, Quaternion.identity, inputSlot.transform);

                Instantiate(createItemFXPrefab, outputSlot.transform.position, Quaternion.identity, outputSlot.transform);

                var outputItemInst = Instantiate(itemPrefab, outputSlot.transform.position, Quaternion.identity, outputSlot.transform).GetComponent<InventoryItemController>();
                outputItemInst.Construct(outputItem);
            }
        }
    }
}