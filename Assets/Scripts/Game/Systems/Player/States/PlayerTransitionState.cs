using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Systems.Player.States
{
    //Game transitioning time/buildings
    public class PlayerTransitionState : PlayerState
    {
        public AnimationCurve BlinkCurve;
        public float BlinkTime;
        public float HoldBlinkTime;

        public UnityEvent OnBlinkHold;
        
        private float curBlinkTime;
        private float lastCurBlinkTime;

        public VolumeProfile profile;
    
        private float vignettePreStart;

        private Vignette _vignette;
        public override void StateStart()
        {
            if (profile.TryGet(typeof(Vignette), out Vignette obj))
            {
                _vignette = obj;
            }
            else
            {
                Debug.LogError("No Vignette Detected, ending transition prematurely");
                Finish();
                return;
            }
            curBlinkTime = 0;
            vignettePreStart = _vignette.intensity.value;
            lastCurBlinkTime = 0;
        }

        
        //WHEN FINISHED, IF ITEMS TO PICK UP, SET TO PICK UP
        // OTHERWISE SET TO NOTHING STATE
        public override void StateUpdate()
        {
            curBlinkTime += Time.deltaTime;
            

            if (curBlinkTime >= BlinkTime)
            {
                
                SetBlink(BlinkCurve.Evaluate(curBlinkTime/BlinkTime));

                if (lastCurBlinkTime < BlinkTime)
                {
                     OnBlinkHold?.Invoke();
                }
                
            }else if (curBlinkTime >= BlinkTime + HoldBlinkTime)
            {
                SetBlink(BlinkCurve.Evaluate((curBlinkTime-HoldBlinkTime - BlinkTime)/(BlinkTime)));
            }else if (curBlinkTime >= BlinkTime * 2 + HoldBlinkTime)
            {
                Finish();
            }
            else
            {
                SetBlink(BlinkCurve.Evaluate(curBlinkTime/BlinkTime));
            }

            lastCurBlinkTime = curBlinkTime;
        }

        private void SetBlink(float amount)
        {
            Mathf.Clamp(amount, 0, 1);
            _vignette.intensity.value = (_vignette.intensity.max - _vignette.intensity.min) * amount;
        }

        public override void OnInteract(IInteractable interacted)
        {
            
        }

        public override void StateStop()
        {
            _vignette.intensity.value = vignettePreStart;
        }
    }
}