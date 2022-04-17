using System;
using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.Player
{

    public enum PlayerStateTypes
    {
        DEFAULT, INVENTORY, TRANSITION, NOTHING, LOOK, TITLE, INTERACT
    }
    
    public abstract class PlayerState : MonoBehaviour
    {
        public UnityEvent OnStateEnter, OnStateLeave;

        [Serializable]
        public struct StateSettings
        {
            [Header("State Settings")]
            public PlayerStateTypes StateID;
            public PlayerStateTypes NextState;

            [HideInInspector]
            public bool UseProceduralNextState;
            [HideInInspector]
            public PlayerStateTypes ProceduralNextState;

            [Space(4)] [Header("Interaction Settings")]
            public PlayerInteractionSettings interactionSettings;

            public bool AllowInventoryOpen;
            public bool AllowUIInteraction;

            [Range(0 , 1)]
            [Tooltip("0 - Free Look 1 - Center Locked")]
            public float LookLimit;
        }

        public enum StateActive
        {
            WAITING, ACTIVE, FINISHED, 
        }
        
        
        public StateActive ActiveState;


        public StateSettings settings;
        public abstract void StateStart();
        
        public abstract void StateUpdate();

        public abstract void OnInteract(IInteractable interacted);

        public virtual void OnSelect(IInteractable interacted)
        {
            
        }

        public virtual void OnDeselect(IInteractable interacted)
        {
            
        }

        public abstract void StateStop();
    
        public void Finish()
        {
            ActiveState = StateActive.FINISHED;
        }
        
        public void Finish(PlayerStateTypes nextState)
        {
            settings.UseProceduralNextState = true;
            settings.ProceduralNextState = nextState;
            
            Finish();
        }
    }
}
