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
    public class InventoryItemsManager : MonoBehaviour
    {
        public static InventoryItemsManager Instance { get; set; }
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
        [SerializeField]
        private Vector2 tooltipOffset;

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
            tooltipTitle.text = itemData.Name;
            tooltipDesc.text = itemData.Tooltip;

            // Refresh everyone
            tooltipDesc.GetComponent<TextSizer>().Refresh();
            tooltipTitle.GetComponent<TextSizer>().Refresh();
            tooltipDesc.GetComponent<TextSizerFillWidth>().Refresh();

            tooltip.SetActive(true);
        }

        public void ClearTooltip()
        {
            tooltip.SetActive(false);
        }

        public void MoveTooltip(Vector2 position)
        {
            tooltip.transform.position = position + tooltipOffset;
        }
    }
}