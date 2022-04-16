using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class FeedInteractable : MonoBehaviour,IInteractable
    {
        public GameObject feedIndicator;
        
        public void OnSelect()
        {
            feedIndicator.SetActive(true);
        }

        public void OnDeselect()
        {
            
            feedIndicator.SetActive(false);
        }

        public void Interact()
        {
            // Do nothing, handled else where
        }

        public InteractType GetInteractType()
        {
            return InteractType.Feed;
        }
    }
}