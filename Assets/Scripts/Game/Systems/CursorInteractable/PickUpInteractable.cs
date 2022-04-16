using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class PickUpInteractable : MonoBehaviour, IInteractable
    {
        public InventoryItem itemReference;
        public GameObject root;
        public GameObject modelObject;
        public InventorySO inventory;
        
        public void OnSelect()
        {
            if(modelObject != null)
            modelObject.layer = LayerMask.NameToLayer("Highlight");
        }

        public void OnDeselect()
        {
            if(modelObject != null)
            modelObject.layer = LayerMask.NameToLayer("Default");
        }

        public void Interact()
        {
            inventory.AddItem(itemReference);
            Destroy(this.root);
        }

        public InteractType GetInteractType()
        {
            return InteractType.PickUp;
        }
    }
}