using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    [CreateAssetMenu(fileName = "DayNightSettings", menuName = "Game/Day Night Settings")]
    public class DayNightSettings : ScriptableObject
    {
        [Serializable]
        public struct TimeOfDaySetting
        {
            public string name;
            public Material skybox;
            public GameObject lighting;
        }
        
        public int CurrentDay;

        public int TimeOfDayIndex;

        public delegate void OnTimeChange(int time);

        public OnTimeChange OnTimeChangeEvent;

        [SerializeField]
        private List<TimeOfDaySetting> timeOfDaySettings = new List<TimeOfDaySetting>();
    
        

        public TimeOfDaySetting Progress()
        {
            if (TimeOfDayIndex > timeOfDaySettings.Count - 2)
            {
                CurrentDay++;
                TimeOfDayIndex = 0;
            }
            else
            {
                TimeOfDayIndex++;
            }

            try
            {
                OnTimeChangeEvent?.Invoke(TimeOfDayIndex);
            }catch(Exception e){}
            
            return timeOfDaySettings[TimeOfDayIndex];
        }

        public TimeOfDaySetting GetCurrent()
        {
            return timeOfDaySettings[TimeOfDayIndex];
        }

        public void Reset()
        {
            CurrentDay = 0;
            TimeOfDayIndex = 0;
        }

    }
}
