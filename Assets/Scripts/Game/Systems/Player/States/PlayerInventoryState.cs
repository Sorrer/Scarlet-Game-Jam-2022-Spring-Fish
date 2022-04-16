using Game.Systems.CursorInteractable;
using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.Player.States
{
    public class PlayerInventoryState : PlayerState
    {
        public GameObject InventoryRoot;
        public InventorySO inventory;
        public PlayerCursorData cursor;

        public PlayerController controller;
        
        public override void StateStart()
        {
            InventoryRoot.SetActive(true);
            cursor.cursorGraphicType = PlayerCursorData.CursorGraphicType.DEFAULT;
        }

        
        //CHANGE CURSOR MANUALLY THROUGH THIS STATE
        public override void StateUpdate()
        {
            //Check if mouse is on anything specific, if so change mouse cursor accordingly
            //Add event callbacks 
            //If right click, close UI

            if (controller.HoveringOverUI)
            {
                cursor.cursorGraphicType = PlayerCursorData.CursorGraphicType.SELECT;
            }
            else
            {
                
                cursor.cursorGraphicType = PlayerCursorData.CursorGraphicType.DEFAULT;
            }

            if (Input.GetMouseButtonDown(1))
            {
                InventoryItem currentlyHeld = inventory.HeldItem;
                
                if (currentlyHeld != null)
                {
                    Finish(PlayerStateTypes.INTERACT);
                }
                else
                {
                    Finish(PlayerStateTypes.INTERACT);
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