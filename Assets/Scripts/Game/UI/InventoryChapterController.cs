using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI.Book
{
    public class InventoryChapterController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private ChapterController chapterController;
        [SerializeField]
        private InventorySO inventorySO;
        [SerializeField]
        private GameObject inventoryPagePrefab;
        [SerializeField]
        private InventoryItemSlotController heldItemSlot;
        [SerializeField]
        private ParticleSystem removeParticle;
        [SerializeField]
        private GameObject itemPrefab;

        private Coroutine heldItemCoroutine;

        private void Awake()
        {
            chapterController.OnPreload.AddListener(OnPreload);
            heldItemSlot.OnItemChanged.AddListener(HeldItemChanged);
        }

        public void Unload()
        {
            if (heldItemSlot.ItemController != null)
            {
                //removeParticle.Play();
                Destroy(heldItemSlot.ItemController.gameObject);
            }
        }

        private void HeldItemChanged()
        {
            if (heldItemSlot.ItemController == null)
                inventorySO.HeldItem = null;
            else
                inventorySO.HeldItem = heldItemSlot.ItemController.ItemData;
        }

        private void OnPreload()
        {
            foreach (var pageContent in chapterController.PageContentList)
                Destroy(pageContent.gameObject);

            chapterController.PageContentList.Clear();
            
            var sortedItems = inventorySO.GetSortedItems();

            if (heldItemSlot.ItemController != null)
                Destroy(heldItemSlot.ItemController.gameObject);

            if (inventorySO.HeldItem != null)
            {
                sortedItems.Remove(inventorySO.HeldItem);
            }
            
            if (heldItemCoroutine != null)
                StopCoroutine(heldItemCoroutine);
            heldItemCoroutine = StartCoroutine(HeldItemEnum());

            var sortedItemsQueue = new Queue<InventoryItem>(sortedItems);

            while (sortedItemsQueue.Count > 0)
            {
                var page = Instantiate(inventoryPagePrefab, chapterController.transform.position, chapterController.transform.rotation, chapterController.transform);
                page.GetComponent<InventoryPageController>().AddItemsFromQueue(sortedItemsQueue);
                chapterController.PageContentList.Add(page);
            }

            if (chapterController.PageContentList.Count == 0)
            {
                var page = Instantiate(inventoryPagePrefab, chapterController.transform.position, chapterController.transform.rotation, chapterController.transform);
                chapterController.PageContentList.Add(page);
            }

            if (chapterController.PageContentList.Count == 1)
            {
                var page = Instantiate(inventoryPagePrefab, chapterController.transform.position, chapterController.transform.rotation, chapterController.transform);
                chapterController.PageContentList.Add(page);
            }
        }

        private IEnumerator HeldItemEnum()
        {
            yield return new WaitForSeconds(0.5f);
            if (inventorySO.HeldItem != null)
            {
                var itemInst = Instantiate(itemPrefab, heldItemSlot.transform.position, heldItemSlot.transform.rotation, heldItemSlot.transform).GetComponent<InventoryItemController>();
                itemInst.Construct(inventorySO.HeldItem);
                heldItemSlot.ItemController = itemInst;
            }
        }
    }
}