using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.TimeOfDay
{
    public class DayNightUnityEvent : MonoBehaviour
    {
        public DayNightSettings settings;


        public List<UnityEvent> TimeChangeUnityEvents = new List<UnityEvent>(); 

        private void Start()
        {
            settings.OnTimeChangeEvent += OnTimeChange;
        }

        public void OnTimeChange(int time)
        {
            if (time < TimeChangeUnityEvents.Count)
            {
                TimeChangeUnityEvents[time]?.Invoke();
            }
        }

        private void OnDestroy()
        {
            settings.OnTimeChangeEvent -= OnTimeChange;
        }
    }
}