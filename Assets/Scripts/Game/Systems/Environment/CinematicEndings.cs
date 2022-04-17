using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Systems.Environment
{
    public class CinematicEndings : MonoBehaviour
    {
        public List<PlayableDirector> playables = new List<PlayableDirector>();

        private bool played = false;
        
        public void Play(int index)
        {
            if (played)
            {
                Debug.LogError("Tried to play another cinematic ending, when only one should be played");
                return;
            }

            played = true;
            playables[index].Play();
        }
    }
}