using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.CursorInteractable
{

    public enum InteractType
    {
        PickUp,Feed,Build
    }
    public interface IInteractable
    {
        void OnSelect();
        void OnDeselect();
        void Interact();

        InteractType GetInteractType();
    }
}
