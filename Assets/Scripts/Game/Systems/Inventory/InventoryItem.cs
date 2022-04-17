using System;
using System.Collections.Generic;
using Game.Systems.Environment;
using UnityEngine;

namespace Game.Systems.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "Game/Item")]
    public class InventoryItem : ScriptableObject
    {
        public Sprite Icon;
        public GameObject Prefab;
        public ItemCategories Category;
        public string Name;
        public string Tooltip;

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{name} - {Tooltip}";

        }

        [Serializable]
        public struct BuildingSettings
        {
            [Tooltip("If null, uses default")]
            public List<DynamicForest.ProgressionEmersion> forestSettings;
            // TODO Create an asset for events and it will call it
            public bool IsEndGame;
            [Tooltip("If null, uses default")]
            public Material waterMaterial;
            [Tooltip("If null, uses default")]
            public Material groundMaterial;

            public int SetPoundState;

            public void Apply(Renderer water, Renderer ground, EnvironmentState pondState)
            {
                
                
                if (waterMaterial != null) water.material = waterMaterial;
                if (groundMaterial != null) ground.material = groundMaterial;
                
                pondState.Activate(SetPoundState);
            }
        }
        [Space(4)]
        [Header("Building Settings")] public BuildingSettings buildingSettings;
        
    }
}