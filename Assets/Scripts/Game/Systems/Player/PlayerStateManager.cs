using System;
using System.Collections.Generic;
using Game.Systems.CursorInteractable;
using UnityEngine;

namespace Game.Systems.Player
{
    public class PlayerStateManager : MonoBehaviour
    {

        public List<PlayerState> states = new List<PlayerState>();

        public PlayerState currentState = null;

        public CursorInteractor cursorInteractor;
        public PlayerController controller;
        
        
        
        private void Start()
        {
            if (currentState == null)
            {
                Debug.LogWarning("No player state set. Please set state. If pausing just disable script");
                return;
            }   
            
            SetState(currentState.settings.StateID, true);
        }

        private void Update()
        {
            if (currentState == null)
            {
                Debug.LogWarning("No player state set. Please set state. If pausing just disable script");
                return;
            }

            if (currentState.settings.interactionSettings.AutoChangeCursor)
            {
                // TODO change to optimized events
                if (cursorInteractor.HasSelected)
                {
                    cursorInteractor.data.cursorGraphicType = currentState.settings.interactionSettings.OnInteractType;
                }
                else
                {
                    cursorInteractor.data.cursorGraphicType = currentState.settings.interactionSettings.cursorGraphicType;
                }
            }

            currentState.StateUpdate();

            if (currentState.settings.AllowInventoryOpen)
            {
                if (Input.GetMouseButtonDown(1) && currentState.settings.StateID != PlayerStateTypes.INVENTORY)
                {
                    SetState(PlayerStateTypes.INVENTORY);
                }
            }
            
            switch (currentState.ActiveState)
            {
                case PlayerState.StateActive.ACTIVE:
                    // Do nothing
                    break;
                
                case PlayerState.StateActive.FINISHED:
                    NextState();
                    break;
                
                case PlayerState.StateActive.WAITING:
                    Debug.LogError("Current state was set to waiting, invalid should not be waiting");
                    break;
                
                default:
                    Debug.LogWarning("State of current node unimplemented");
                    break;
            }
            
            
            
        }

        private void NextState()
        {
            if (currentState.settings.UseProceduralNextState)
            {
                currentState.settings.UseProceduralNextState = false;
                SetState(currentState.settings.ProceduralNextState);
            }
            else
            {
                SetState(currentState.settings.NextState);
            }
            
        }

        public void SetState(PlayerStateTypes type, bool ignoreLastState = false)
        {
            
            var lastState = currentState;
            
            if (ignoreLastState)
            {
                lastState = null;
            }
            
            if (lastState != null)
            {
                lastState.ActiveState = PlayerState.StateActive.WAITING;
                lastState.OnStateLeave?.Invoke();
                lastState.StateStop();
            }
            
            currentState = GetState(type);
            currentState.OnStateEnter?.Invoke();
            currentState.StateStart();
            currentState.ActiveState = PlayerState.StateActive.ACTIVE;
            controller.AllowUIClick = currentState.settings.AllowUIInteraction;
            controller.cameraController.LimitLookRange = currentState.settings.LookLimit;
            
            SetupCursor(lastState,currentState);
        }

        public void SetupCursor(PlayerState lastState, PlayerState state)
        {
            var settings = state.settings.interactionSettings;
            
            cursorInteractor.enabled = settings.EnableInteractions;

            if (lastState != null)
            {
                cursorInteractor.OnInteractEvent -= lastState.OnInteract;
                cursorInteractor.OnSelectEvent -= lastState.OnSelect;
                cursorInteractor.OnDeselectEvent -= lastState.OnDeselect;
            }
            
            if (settings.SingleInteraction)
            {
                cursorInteractor.Activate(settings.InteractionLayer, state.OnInteract);
            }
            else
            {
                cursorInteractor.OnInteractEvent += state.OnInteract;
            }
            
            cursorInteractor.OnSelectEvent += state.OnSelect;
            cursorInteractor.OnDeselectEvent += state.OnDeselect;
            
        }

        private PlayerState GetState(PlayerStateTypes type)
        {
            foreach (var state in states)
            {
                if (state == null) continue;
                if (state.settings.StateID == type) return state;
            }
            
            Debug.LogError("State not found! Current State - "  + currentState.settings.StateID +", DEFAULTING TO 0 " + type);
            return states[0];
        }

        private void OnValidate()
        {
            //Check for duplicates
            
            HashSet<PlayerStateTypes> currentTypes = new HashSet<PlayerStateTypes>();
            for (int i = states.Count - 1; i >= 0; i--)
            {
                var state = states[i];
                if (state == null) continue;
                
                if (currentTypes.Contains(state.settings.StateID))
                {
                    Debug.LogWarning("State duplicate detected, removing duplicate " + state.settings.StateID + " - " + state.name);
                    states[i] = null;
                }
                else
                {
                    currentTypes.Add(state.settings.StateID);
                }
                
                
            }
        }

    }
}