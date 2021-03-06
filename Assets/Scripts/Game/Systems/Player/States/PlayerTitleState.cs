using Game.Systems.CursorInteractable;
using Game.Systems.Inventory;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Systems.Player.States
{
    public class PlayerTitleState : PlayerState
    {

        public PlayableDirector playableDirector;
        public GameObject titleScreen;
        public DayNightSettings dayNightSettings;

        public InventorySO inventory;
        public InventoryItem startFood;
        
        private bool _startGame = false;
        private float curTime;
        public override void StateStart()
        {
            //Disable title UI
            _startGame = false;
            titleScreen.SetActive(true);
        }

        public void OnStartClicked()
        {
            _startGame = true;
            if(playableDirector != null) playableDirector.Play();
        }
        
        public override void StateUpdate()
        {
            
            if (_startGame)
            {
                if (playableDirector == null)
                {
                    Debug.LogWarning("Title screen does not have a playable director, starting anyways");
                    Finish();
                    return;
                }
                
                if (playableDirector.state != PlayState.Playing)
                {
                    Finish();
                }
            }
        }

        public override void OnInteract(IInteractable interacted)
        {
            
        }

        public override void StateStop()
        {
            inventory.AddItem(startFood);
            dayNightSettings.CurrentDay = 0;
            dayNightSettings.TimeOfDayIndex = -1;
            titleScreen.SetActive(false);
        }
    }
}