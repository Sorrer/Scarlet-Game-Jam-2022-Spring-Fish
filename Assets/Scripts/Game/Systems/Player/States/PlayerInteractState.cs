using Game.Systems.CursorInteractable;

namespace Game.Systems.Player.States
{
    //Combination of Feed Build and Pickup
    public class PlayerInteractState : PlayerState
    {
        public CursorInteractor interactor;
        
        public override void StateStart()
        {
            
        }

        public override void OnSelect(IInteractable interacted)
        {
            base.OnSelect(interacted);

            switch (interacted.GetInteractType())
            {
                case InteractType.Build:
                    interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.BUILD;
                    break;
                case InteractType.Feed:
                    interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.FEED;
                    break;
                case InteractType.PickUp:
                    interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.PICKUP;
                    break;
            }
        }

        public override void OnDeselect(IInteractable interacted)
        {
            base.OnDeselect(interacted);
            interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.SELECT_CIRCLE;
        }

        public override void OnInteract(IInteractable interacted)
        {
            
        }

        public override void StateUpdate()
        {
            
        }


        public override void StateStop()
        {
        }
    }
}