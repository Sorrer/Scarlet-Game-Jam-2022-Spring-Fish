using System;
using System.Collections.Generic;
using System.Linq;
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="progression"></param>
        /// <param name="density">0 - 1</param>
        public void Set(List<ProgressionEmersion> progression, float density)
        {                                                                                                                                                                             
            int total = 0;
                
            for (int i = 0; i < progression.Count; i++)
            {
                total += progression[i].SpawnWeight;
            }

            int totalElements = elements.Count;
            int totalElementsDisabled = (int) ((int) elements.Count * Mathf.Clamp(density, 0, 1));
            for (int i = 0; i < elements.Count; i++)
            {
                
                elements[i].gameObject.SetActive(true);
                elements[i].SetProgressionStage(GetStage(progression, total));
            }

            List<ProgressionElement> elementsCopy = elements.ToList();

            while (elementsCopy.Count > 0 || totalElementsDisabled > 0)
            {
                int randomIndex = Random.Range(0, elementsCopy.Count);
                
                elementsCopy[randomIndex].gameObject.SetActive(false);
                
                totalElementsDisabled--;
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