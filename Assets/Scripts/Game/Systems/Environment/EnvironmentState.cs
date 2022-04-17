using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Environment
{
    public class EnvironmentState : MonoBehaviour
    {
        

        public List<GameObject> groups = new List<GameObject>();

        public void Activate(int index)
        {
            if (groups.Count >= index || index < 0)
            {
                Debug.LogError("Tried to activate out of bounds, doing nothing");
                return;
            }

            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].SetActive(false);
            }
            
            groups[index].SetActive(true);
        }
    }
}