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

        public void Set(ProgressionElement.ProgressionStage stage, ProgressionElement.ProgressionStage lastStage,
            float emersion)
        {
            //Set all stages to either stage or last stage with emersion (0-1) being how many of last stage to include in new stage
        }
    }
}