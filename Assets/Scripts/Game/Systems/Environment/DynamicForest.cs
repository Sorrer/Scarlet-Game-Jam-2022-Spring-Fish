using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Systems.Environment
{
    public class DynamicForest : MonoBehaviour
    {
        public static DynamicForest instance;

        [HideInInspector] public List<ProgressionElement> elements = new List<ProgressionElement>();
        
        private void Awake()
        {
            if (instance != this)
            {
                instance = this;
            }
            else
            {
                if (instance != null)
                {
                    Debug.Log("Two instance of dynamic forest found. This is not gud");
                }
            }
        }

        public struct ProgressionEmersion
        {
            public float EmersionRate;
            public ProgressionElement.ProgressionStage stage;
        }
        
        public void Set(List<ProgressionEmersion> progression)
        {
            float total = 0;

            for (int i = 0; i < progression.Count; i++)
            {
                total += progression[i].EmersionRate;
            }
        }
    }
}