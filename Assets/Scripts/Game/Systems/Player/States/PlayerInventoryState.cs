using Game.Systems.CursorInteractable;
using UnityEngine;

namespace Game.Systems.Player.States
{
    public class PlayerInventoryState : PlayerState
    {
        public GameObject InventoryRoot;
        
        public override void StateStart()
        {
            InventoryRoot.SetActive(true);
        }

        
        //CHANGE CURSOR MANUALLY THROUGH THIS STATE
        public override void StateUpdate()
        {
            //Check if mouse is on anything specific, if so change mouse cursor accordingly
            //Add event callbacks 
            //If right click, close UI


            if (Input.GetMouseButton(1))
            {
                IInteractable currentlyHeld = null;

                if (currentlyHeld != null)
                {
                    Finish(PlayerStateTypes.INVENTORY);
                }
                else
                {
                    Finish(PlayerStateTypes.LOOK);
                }
            }
        }

        public override void OnInteract(IInteractable interacted)
        {
            
        }

        public override void StateStop()
        { 
            InventoryRoot.SetActive(false);
        }
    }
}