using System;
using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Game.Systems.Player.States
{
    //Game transitioning time/buildings
    public class PlayerTransitionState : PlayerState
    {
        public AnimationCurve BlinkCurve;
        public float BlinkTime;
        public float HoldBlinkTime;

        public DayNightManager dayNightManager;
        public CursorInteractor interactor;

        public UnityEvent OnBlinkHold;
        public UnityEvent OnBlinkOpen;
        public delegate void OnBlink();

        /// <summary>
        /// Is cleared after blink
        /// </summary>
        public OnBlink OnceOnBlink;
        
        public Image blinkImage;
        
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
            vignettePreStart = 0.331f;
            lastCurBlinkTime = 0;
            interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.EYE_CLOSED;
        }

        
        //WHEN FINISHED, IF ITEMS TO PICK UP, SET TO PICK UP
        // OTHERWISE SET TO NOTHING STATE
        public override void StateUpdate()
        {
            curBlinkTime += Time.deltaTime;
            
            if (curBlinkTime >= BlinkTime * 2 + HoldBlinkTime)
            {
                Finish();
                
            }
            else if (curBlinkTime >= BlinkTime + HoldBlinkTime)
            {
                SetBlink(BlinkCurve.Evaluate(1 - (curBlinkTime-HoldBlinkTime - BlinkTime)/(BlinkTime)));
                interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.EYE;
            }else if (curBlinkTime >= BlinkTime)
            {
                
                SetBlink(BlinkCurve.Evaluate(curBlinkTime/BlinkTime));

                if (lastCurBlinkTime < BlinkTime)
                {
                     OnBlinkHold?.Invoke();
                     dayNightManager.Progress();
                     OnceOnBlink?.Invoke();
                     OnceOnBlink = null;
                }
                
            }else
            {
                SetBlink(BlinkCurve.Evaluate(curBlinkTime/BlinkTime));
            }

            lastCurBlinkTime = curBlinkTime;
        }

        private void SetBlink(float amount)
        {
            Mathf.Clamp(amount, 0, 1);
            _vignette.intensity.value = (_vignette.intensity.max - _vignette.intensity.min) * amount * 2;
            blinkImage.color = new Color(0, 0, 0, amount);
        }

        public override void OnInteract(IInteractable interacted)
        {
            
        }

        public override void StateStop()
        {
            _vignette.intensity.value = vignettePreStart;
        }

        private void OnDestroy()
        {
            if (ActiveState == StateActive.ACTIVE)
            {
                _vignette.intensity.value = vignettePreStart;
            }
        }
    }
}