using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public interface IInteractable
    {
        void OnSelect();
        void OnDeselect();
        void Interact();
    }
}
