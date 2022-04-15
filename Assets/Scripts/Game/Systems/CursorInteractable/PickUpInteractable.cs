using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class PickUpInteractable : MonoBehaviour, IInteractable
    {

        public GameObject modelObject;
        
        public void OnSelect()
        {
            modelObject.layer = LayerMask.NameToLayer("Highlight");
        }

        public void OnDeselect()
        {
            modelObject.layer = LayerMask.NameToLayer("Default");
        }

        public void Interact()
        {
            Debug.Log("Interacted");
        }

        public InteractType GetInteractType()
        {
            return InteractType.PickUp;
        }
    }
}