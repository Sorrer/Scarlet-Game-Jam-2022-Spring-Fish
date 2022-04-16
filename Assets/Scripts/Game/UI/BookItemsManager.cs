using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class BookItemsManager : MonoBehaviour
    {
        public static BookItemsManager Instance { get; set; }
        public Transform DragDropHolder { get => dragDropHolder; set => dragDropHolder = value; }

        [Header("Dependencies")]
        [SerializeField]
        private Transform dragDropHolder;
       
        [Header("Tooltip")]
        [SerializeField]
        private GameObject tooltip;
        [SerializeField]
        private TextMeshProUGUI tooltipTitle;
        [SerializeField]
        private TextMeshProUGUI tooltipDesc;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            tooltip.SetActive(false);
        }

        public void DisplayTooltip(InventoryItem itemData)
        {
            tooltipTitle.text = itemData.name;
            tooltipDesc.text = itemData.Tooltip;
            tooltip.SetActive(true);
        }

        public void ClearTooltip()
        {
            tooltip.SetActive(false);
        }
    }
}