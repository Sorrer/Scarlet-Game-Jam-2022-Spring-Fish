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
        private ParticleSystem[] craftInputParticles;
        [SerializeField]
        private ParticleSystem[] craftInputRemoveParticles;
        [SerializeField]
        private ParticleSystem craftOutputParticle;
        [SerializeField]
        private ParticleSystem craftOutputRemoveParticle;
        [SerializeField]
        private AudioSource craftAudio;
        [SerializeField]
        private InventorySO inventorySO;
        [SerializeField]
        private CraftingList craftingList;

        private void Awake()
        {
            foreach (var ingredientSlot in inputSlots)
                ingredientSlot.OnItemChanged.AddListener(TryCraft);
        }

        public void Unload()
        {
            int i = 0;
            foreach (var inventorySlot in inputSlots)
            {
                if (inventorySlot.ItemController != null)
                {
                    //craftInputRemoveParticles[i].Play();
                    Destroy(inventorySlot.ItemController.gameObject);
                }
                i++;
            }

            if (outputSlot.ItemController != null)
            {
                //craftOutputRemoveParticle.Play();
                Destroy(outputSlot.ItemController.gameObject);
            }
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
                for (int i = 0; i < inputSlots.Count; i++)
                {
                    var slot = inputSlots[i];
                    if (slot.ItemController != null)
                    {
                        inventorySO.RemoveItem(slot.ItemController.ItemData);
                        Destroy(slot.ItemController.gameObject);
                        craftInputParticles[i].Play();
                    }
                }
                craftOutputParticle.Play();
                craftAudio.Play();

                var outputItemInst = Instantiate(itemPrefab, outputSlot.transform.position, outputSlot.transform.rotation, outputSlot.transform).GetComponent<InventoryItemController>();
                outputItemInst.Construct(outputItem);
                outputSlot.ItemController = outputItemInst;
                inventorySO.AddItem(outputItem);
            }
        }
    }
}