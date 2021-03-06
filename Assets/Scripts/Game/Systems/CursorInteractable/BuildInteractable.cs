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
        private void Awake()
        {
            buildingVisuals = this.GetComponent<BuildingVisuals>();
            root = this.transform.parent.gameObject;
            root.SetActive(false);
        }

        public void Activate()
        {
            if(placed) Debug.LogError("Placing an already placed building");
            
            if(root != null) root.SetActive(true);
            buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Activate);
        }
        
        public void OnSelect()
        {
            if(!placed) buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Hover);
        }

        public void OnDeselect()
        {
            if(!placed) buildingVisuals.visualsControl(BuildingVisuals.EffectSelect.Unhover);
        }

        public void Interact()
        {
            placed = true;
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
            if(!placed && root != null)
            root.SetActive(false);
        }

        public InteractType GetInteractType()
        {
            return InteractType.Build;
        }
    }
}