using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory.Progression
{
    public class FishFeedingList : ScriptableObject
    {
        public class ItemsOrderNode
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
