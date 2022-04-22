using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory.Progression
{
    public class FishAnimator : MonoBehaviour
    {


        public bool IsFinished = false;
        
        private FishAnimationData currentFish = null;
        public void StartFish(GameObject fishObj)
        {
            // Instantiate fish, check if it has fish bones, if not dont start

            if (currentFish != null)
            {
                IsFinished = true;
                return;
            }

            var go = Instantiate(fishObj);
            currentFish = go.GetComponent<FishAnimationData>();
            
            if (currentFish == null)
            {
                Destroy(go);
                IsFinished = true;
                return;
            }

            IsFinished = false;
            currentFish.StartFish();
        }

        private void Update()
        {
            if (currentFish != null && currentFish.IsFinished)
            {
                IsFinished = true;
                currentFish = null;
            }
        }
    }
}