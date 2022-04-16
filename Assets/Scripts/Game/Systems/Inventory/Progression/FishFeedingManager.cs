﻿using System.Collections;
using System.Collections.Generic;
using Game.Systems.Player;
using Game.Systems.Player.States;
using UnityEngine;

namespace Game.Systems.Inventory.Progression
{
    public class FishFeedingManager : MonoBehaviour
    {
        // Get next item
        // Check if inventory already has item
        // If so go to the next item
        // is repeat is reached twice, stop and no item gets returned

        public Transform itemSpawnLoc;
        public Transform feedSpawnLoc;

        public float timeToActivateFishy;
        
        public GameData data;
        public PlayerTransitionState transitionState;
        public PlayerStateManager stateManager;

        public FishAnimator fishAnimator;
        
        public List<FishFeedingList> fishFeedingLists = new List<FishFeedingList>();
        public InventorySO inventory;
        public bool IsRunning { get; private set;  }

        public void FeedFish()
        {
            FeedFish(inventory.HeldItem);
        }
        public void FeedFish(InventoryItem item)
        {
            if (IsRunning)
            {
                Debug.LogError("Tried to feed fish while already feeding fish");
                return;
            }
            
            IsRunning = true;

            var list = GetList(item);

            if (list == null)
            {
                Debug.LogError("Invalid item go fed, this should not happen, forcing done");
                StartFeedingAnimation(null, list);
                return;
            }



            FishFeedingList.ItemsOrderNode returnItem = NextItem(list);

            var firstItemNode = returnItem;
            //Check if there is an item that the player doesnt have
            while (inventory.HasItem(returnItem.item))
            {
                returnItem = NextItem(list);

                if (firstItemNode.item == returnItem.item)
                {
                    Debug.Log("Already has item, not spawning");
                    StartFeedingAnimation(null, list);
                    return;
                }
            }
            
            StartFeedingAnimation(returnItem.item, list);
            
        }

        public void StartFeedingAnimation(InventoryItem returnItem, FishFeedingList list)
        {

            Debug.Log($"Spawning fish + food + return item {list.FishPrefab} {list.FeedItem.name} {returnItem.name}");
            StartCoroutine(DoFeedingAnimation(returnItem, list));
        }

        private InventoryItem spawnThisItem;
        public IEnumerator DoFeedingAnimation(InventoryItem returnItem, FishFeedingList list)
        {
            var feed = Instantiate(list.FeedItem.Prefab, feedSpawnLoc);
            
            yield return new WaitForSeconds(timeToActivateFishy);

            fishAnimator.StartFish(list.FishPrefab);
            while (!fishAnimator.IsFinished)
            {
                yield return null;
            }
            
            yield return null;

            spawnThisItem = returnItem;
            transitionState.OnBlinkHold.AddListener(FinishFeeding);
            
            Destroy(feed);
            stateManager.SetState(PlayerStateTypes.TRANSITION);
        }

        public void FinishFeeding()
        {
            //If there is no item to spawn do not spawn
            // TODO: ADD NO ITEMS SPAWNED EFFECT
            //
            
            var go =Instantiate(spawnThisItem.Prefab, itemSpawnLoc);
            go.transform.position = itemSpawnLoc.position;
            
            IsRunning = false;
            transitionState.OnBlinkHold.RemoveListener(FinishFeeding);
            
            Debug.Log("Finished feeding " + spawnThisItem);
        }



        public FishFeedingList GetList(InventoryItem fishFood)
        {
            foreach (var list in fishFeedingLists)
            {
                if (list.FeedItem == fishFood)
                {
                    return list;
                }
            }
            
            Debug.LogError("Could not find fish list for food item " + fishFood.Name);
            return null;
        }

        public FishFeedingList.ItemsOrderNode NextItem(FishFeedingList fishFood)
        {
            var index = fishFood._currentIndex;
            var list = fishFood.itemsReceiveOrder;
            
            //Increment
            
            if (index + 1 >= list.Count)
            {
                if (list[list.Count - 1].GoBackToIndex)
                {
                    index = list[list.Count - 1].GoBackIndex;
                }
                else
                {
                    index = 0;
                }
            }
            else
            {

                if (list[index].GoBackToIndex)
                {
                    index = list[index].GoBackIndex;
                }
                else
                {
                    index++;
                }
            }

            
            
            return list[index];
        }
    }
}