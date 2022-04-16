using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemSlotController : MonoBehaviour
    {   
        public UnityEvent OnItemChanged;

        [Header("Settings")]
        [SerializeField]
        private bool canAcceptItem = true;
        
        private InventoryItemController itemController;
        public InventoryItemController ItemController { get => itemController; 
            set 
            {
                itemController = value;
                OnItemChanged.Invoke();
            } 
        }
        public bool CanAcceptItem { get => canAcceptItem; set => canAcceptItem = value; }

        private void Awake()
        {
            var childItemController = GetComponentInChildren<InventoryItemController>();
            if (childItemController != null)
                ItemController = childItemController;
        }

        public bool TryAcceptItem(InventoryItemController item)
        {
            if (!CanAcceptItem || ItemController != null)
                return false;

            ItemController = item;
            return true;
        }
    }
}