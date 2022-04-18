using System;
using System.Collections.Generic;
using Game.Systems.CursorInteractable;
using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems.Buildings
{
    public class BuildingList : MonoBehaviour
    {
        [Serializable]
        public struct BuildingListPrefabPair
        {
            public InventoryItem buildingRelation;
            public BuildInteractable interactable;
        }

        public List<BuildingListPrefabPair> pairs = new List<BuildingListPrefabPair>();

        private void Awake()
        {
            DeactivateAll();

            for (int i = 0; i < pairs.Count; i++)
            {
                var pair = 
                    pairs[i];
                    
                pair.buildingRelation= pairs[i].interactable.buildingReference;
                pairs[i] = pair;
            }
        }

        public void Activate(InventoryItem item)
        {
            bool set = false;
            
            foreach (var pair in pairs)
            {
                if (pair.buildingRelation == item)
                {
                    set = true;
                    pair.interactable.Activate();
                }
                else
                {
                    pair.interactable.Deactivate();
                }
            }

            if (!set)
            {
                Debug.LogError("Building does not have a pair. Please fix :D");
            }
        }

        public void DeactivateAll()
        {
            foreach (var pair in pairs)
            {
                if(!pair.interactable.placed) pair.interactable.Deactivate();
            }
            
        }

        public void Place(InventoryItem item)
        {
            bool set = false;
            
            foreach (var pair in pairs)
            {
                if (pair.buildingRelation == item)
                {
                    set = true;
                    pair.interactable.Place();
                }
            }

            if (!set)
            {
                Debug.LogError("Item not placed, please check pairs!");
            }
        }
    }
}