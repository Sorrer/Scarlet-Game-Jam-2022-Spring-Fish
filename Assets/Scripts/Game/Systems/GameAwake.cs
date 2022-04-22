using System;
using Game.Systems.Inventory;
using UnityEngine;

namespace Game.Systems
{
    
    public class GameAwake : MonoBehaviour
    {
        public CraftingList list;

        private void Start()
        {
            list.buildingsBuilt.Clear();
        }
    }
}