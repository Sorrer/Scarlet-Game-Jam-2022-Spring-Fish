using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class BuildInteractable : MonoBehaviour, IInteractable
    {
        public InventoryItem buildingReference;
        public void OnSelect()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        public void Interact()
        {
            throw new System.NotImplementedException();
        }

        public InteractType GetInteractType()
        {
            throw new System.NotImplementedException();
        }
    }
}