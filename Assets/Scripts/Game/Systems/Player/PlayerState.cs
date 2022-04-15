using System;
using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.Player
{

    public enum PlayerStateTypes
    {
        DEFAULT, FEED, BUILD, PICK_UP, INVENTORY, TRANSITION, NOTHING, LOOK, TITLE
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
