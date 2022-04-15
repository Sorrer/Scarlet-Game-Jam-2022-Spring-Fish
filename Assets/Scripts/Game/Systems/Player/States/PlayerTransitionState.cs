using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Systems.Player.States
{
    //Game transitioning time/buildings
    public class PlayerTransitionState : PlayerState
    {
        public AnimationCurve BlinkCurve;
        public float BlinkTime;
        private float curBlinkTime;

        public VolumeProfile profile;
        
        public override void StateStart()
        {
        }

        
        //WHEN FINISHED, IF ITEMS TO PICK UP, SET TO PICK UP
        // OTHERWISE SET TO NOTHING STATE
        public override void StateUpdate()
        {
            curBlinkTime += Time.deltaTime;

            if (profile.TryGet(typeof(Vignette), out Vignette obj))
            {
                
            }
            else
            {
                Debug.LogError("No Vignette Detected, ending transition prematurely");
                Finish();
                return;
            }

            if (curBlinkTime >= BlinkTime)
            {
                
            }
            
        }

        public override void OnInteract(IInteractable interacted)
        {
        }

        public override void StateStop()
        {
        }
    }
}