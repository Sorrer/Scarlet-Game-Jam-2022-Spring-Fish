using Game.Systems.Player;
using UnityEngine;

namespace Game.Systems.CursorInteractable
{
    public class CursorInteractor : MonoBehaviour
    {

        public PlayerCursorData data;
        public Camera mainCamera;    
    
        public LayerMask layerMask;

        public delegate void OnInteract(IInteractable interactable);

        private OnInteract singleInteractEvent;
        public OnInteract OnInteractEvent;
        public OnInteract OnSelectEvent;
        public OnInteract OnDeselectEvent;
        

        private IInteractable lastInteractable;
    
        public bool HasSelected => lastInteractable != null;

        
        
        void Update()
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(data.position), out var hit, Mathf.Infinity, layerMask))
            {

                var interactor = hit.collider.GetComponent<IInteractable>();

                if (interactor != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        interactor.Interact();
                        OnInteractEvent?.Invoke(interactor);
                        if (singleInteractEvent != null)
                        {
                            singleInteractEvent?.Invoke(interactor);
                            Deactivate();
                        }
                    }

                    if (lastInteractable != interactor)
                    {
                        if (lastInteractable != null)
                        {
                            OnDeselectEvent?.Invoke(lastInteractable);
                            lastInteractable.OnDeselect();
                        }
                        
                        interactor.OnSelect();
                        OnSelectEvent?.Invoke(interactor);
                        
                        lastInteractable = interactor;
                    }
                    

                }


            }
            else
            {
                if (lastInteractable != null)
                {
                    lastInteractable.OnDeselect();
                    lastInteractable = null;
                }
            }

        }

        
        

        /// <summary>
        /// Activates the cursor with a specify layer to look for (picking up/building/placing)
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="callbackInteract">If set, this will activate as a one time usage</param>
        public void Activate(LayerMask interactionLayer, OnInteract callbackInteract = null)
        {
            singleInteractEvent = callbackInteract;
            layerMask = interactionLayer;
            enabled = true;
        }

        public void Deactivate()
        {
            if (lastInteractable != null)
            {
                lastInteractable.OnDeselect();
                lastInteractable = null;
            }
            enabled = false;
        }
    }
}
