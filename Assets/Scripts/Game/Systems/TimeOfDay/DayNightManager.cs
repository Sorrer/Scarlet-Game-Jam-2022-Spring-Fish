using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Game.Systems
{
    public class DayNightManager : MonoBehaviour
    {
        public DayNightSettings settings;

        public GameObject currentLighting;

        private void Start()
        {
            if(currentLighting) Destroy(currentLighting);
            settings.Reset();
            SetDay(settings.GetCurrent());
        }

        public void Progress()
        {
            var newDaySetting = settings.Progress();
            SetDay(newDaySetting);
        }

        private void SetDay(DayNightSettings.TimeOfDaySetting daySetting)
        {
            currentLighting.SetActive(false);
            Destroy(currentLighting);

            currentLighting = Instantiate(daySetting.lighting);

            RenderSettings.skybox = daySetting.skybox;
        }
    }
}
