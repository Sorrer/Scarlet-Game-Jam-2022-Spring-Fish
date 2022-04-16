using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [Serializable]
        public struct ProgressionEmersion
        {
            public int SpawnWeight;
            public ProgressionElement.ProgressionStage stage;
        }
        
        public void Set(List<ProgressionEmersion> progression)
        {                                                                                                                                                                             
            int total = 0;
                
            for (int i = 0; i < progression.Count; i++)
            {
                total += progression[i].SpawnWeight;
            }


            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].SetProgressionStage(GetStage(progression, total));
            }
        }

        private ProgressionElement.ProgressionStage GetStage(List<ProgressionEmersion> progressions, int total)
        {
            int randomValue = (int) Random.value * total;

            for (int i = 0; i < progressions.Count; i++)
            {
                randomValue -= progressions[i].SpawnWeight;
                if (randomValue <= 0)
                {
                }
            }

            return progressions[progressions.Count - 1].stage;
        } 
    }
}