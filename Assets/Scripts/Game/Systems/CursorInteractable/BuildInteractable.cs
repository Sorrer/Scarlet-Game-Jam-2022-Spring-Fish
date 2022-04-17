using System;
using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class BuildInteractable : MonoBehaviour, IInteractable
    {
        public InventoryItem buildingReference;
        public BuildingVisuals buildingVisuals;
        public GameObject root;
        public bool placed = false;
        private void Start()
        {
            root.SetActive(false);
        }

        public void Activate()
        {
            if(placed) Debug.LogError("Placing an already placed building");
            
            root.SetActive(true);
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Activate);
        }
        
        public void OnSelect()
        {
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Hover);
        }

        public void OnDeselect()
        {
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Unhover);
        }

        public void Interact()
        {
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Confirm);
        }

        public void Place()
        {
            placed = true;
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Finish);
            this.gameObject.SetActive(false);
        }

        public void Deactivate()
        {
            if(!placed)
            root.SetActive(false);
        }

        public InteractType GetInteractType()
        {
            return InteractType.Build;
        }
    }
}