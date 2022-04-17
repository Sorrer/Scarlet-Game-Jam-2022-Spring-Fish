﻿using System;
using System.Collections.Generic;
using Game.Systems.CursorInteractable;
using Game.Systems.Environment;
using Game.Systems.Inventory;
using Game.Systems.Inventory.Progression;
using UnityEngine;

namespace Game.Systems.Player.States
{
    //Combination of Feed Build and Pickup
    public class PlayerInteractState : PlayerState
    {
        public CursorInteractor interactor;
        public FishFeedingManager feed;

        public Camera feedCamera;
        public Camera buildCamera;

        public List<LayerMask> feedLayerOrder;
        public List<LayerMask> defaultLayerOrder;

        public GameObject objectToPickup;
        
        public GameObject feedInteraction;

        public Renderer Water, Ground;
        public Material DefaultWater, DefaultGround;
        public EnvironmentState PondEnvironmentState;
        
        public override void StateStart()
        {
            //If held item is feed, allow selection for that + pickup
            //If held item is building, allow selection for that + pickup

            feedCamera.enabled = false;
            buildCamera.enabled = false;
            
            feedInteraction.SetActive(false);
            
            if (feed.inventory.HeldItem != null)
            {
                switch (feed.inventory.HeldItem.Category)
                {
                    case ItemCategories.Buildings:
                        buildCamera.enabled = true;
                        // TODO: Enable specific building that needs to be activated, have a general list for it and find the inventory item that matches;
                        break;
                    case ItemCategories.Food:
                        feedCamera.enabled = true;
                        feedInteraction.SetActive(true);
                        break;
                    case ItemCategories.Craftable:
                    
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override void OnSelect(IInteractable interacted)
        {
            base.OnSelect(interacted);


            interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.INVALID;
            
            var inventoryHeldCategory = feed.inventory.HeldItem;
            
            switch (interacted.GetInteractType())
            {
                case InteractType.Build:
                    if(inventoryHeldCategory != null && inventoryHeldCategory.Category == ItemCategories.Buildings) interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.BUILD;
                    break;
                case InteractType.Feed:
                    if(inventoryHeldCategory != null && inventoryHeldCategory.Category == ItemCategories.Food && objectToPickup == null) interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.FEED;
                    break;
                case InteractType.PickUp:
                    interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.PICKUP;
                    break;
                default:
                    interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.EYE;
                    return;
            }
        }

        public override void OnDeselect(IInteractable interacted)
        {
            base.OnDeselect(interacted);
            if(feed.inventory.HeldItem != null) interactor.data.cursorGraphicType = PlayerCursorData.CursorGraphicType.SELECT_CIRCLE;
        }

        public override void OnInteract(IInteractable interacted)
        {
            if (feed.inventory.HeldItem == null) return;

            var heldItem = feed.inventory.HeldItem;
            
            if (interacted.GetInteractType() == InteractType.Feed && heldItem.Category == ItemCategories.Food)
            {
                if (heldItem != null && objectToPickup == null)
                {
                    //Do the feed stuff
                    feed.FeedFish();
                    feed.inventory.HeldItem = null;
                    Finish(PlayerStateTypes.LOOK);
                }
                else
                {
                    Debug.LogError("Tried to feed without a held item");
                }
                
            }else if (interacted.GetInteractType() == InteractType.Build &&
                      heldItem.Category == ItemCategories.Buildings)
            {
                // TODO
                // Do the building stuff

                Water.material = DefaultWater;
                Ground.material = DefaultGround;

                var settings = heldItem.buildingSettings;
                
                settings.Apply(Water, Ground, PondEnvironmentState);

                if (heldItem.buildingSettings.IsEndGame)
                {
                    // Do end game stuff required by this, IDK to be determined
                    Finish(PlayerStateTypes.LOOK);
                    return;
                }
                
                feed.inventory.HeldItem = null;
                Finish(PlayerStateTypes.INTERACT);
            }


        }

        public override void StateUpdate()
        {
    

        }


        public override void StateStop()
        {
            feedCamera.enabled = false;
            buildCamera.enabled = false;
        }
    }
}