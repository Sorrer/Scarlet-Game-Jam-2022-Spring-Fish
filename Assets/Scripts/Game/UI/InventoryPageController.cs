using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class InventoryPageController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private GameObject itemPrefab;

        private List<InventoryItemSlotController> slots = new List<InventoryItemSlotController>();

        private void Awake()
        {
            foreach (Transform child in transform)
                slots.Add(child.GetComponent<InventoryItemSlotController>());
        }

        public void AddItemsFromQueue(Queue<InventoryItem> items)
        {
            int i = 0;
            foreach (var slot in slots)
            {
                if (items.Count <= 0)
                    return;
                var item = items.Dequeue();
                Instantiate(itemPrefab, slots[i].transform.position, slots[i].transform.rotation, slots[i].transform).GetComponent<InventoryItemController>().Construct(item);
                i++;
            }
        }
    }
}