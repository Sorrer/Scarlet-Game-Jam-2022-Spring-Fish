using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory.Progression
{
    [CreateAssetMenu(fileName = "Fish feeding list", menuName = "Game/Fish Feeding List")]
    public class FishFeedingList : ScriptableObject
    {
        [Serializable]
        public struct ItemsOrderNode
        {
            public InventoryItem item;
            public bool GoBackToIndex;
            public int GoBackIndex;
        }

        [HideInInspector]
        public int _currentIndex;
        
        public GameObject FishPrefab;
        public InventoryItem FeedItem;
    
        public List<ItemsOrderNode> itemsReceiveOrder = new List<ItemsOrderNode>();

        private void Awake()
        {
            _currentIndex = 0;
        }
    }
}
