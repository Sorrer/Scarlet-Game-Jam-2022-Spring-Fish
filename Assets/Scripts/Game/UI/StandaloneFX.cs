using Game.Systems.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class StandaloneFX : MonoBehaviour
    {
        [SerializeField]
        private bool playOnAwake;
        [SerializeField]
        private float lifetime;
        [SerializeField]
        private AudioSource[] audioSources;
        [SerializeField]
        private ParticleSystem[] particleSystems;
        [SerializeField]
        private Animation[] animations;

        private bool playing;
        private float time = 0;

        private void Awake()
        {
            if (playOnAwake)
                Play();
        }

        public void Play()
        {
            if (!playing)
                return;
            playing = true;

            StartCoroutine(DeathEnum());
        }

        private IEnumerator DeathEnum()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
        }
    }
}