using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Player
{
    public class PlayerCursorUI : MonoBehaviour
    {
        public PlayerCursorData data;

        [Serializable]
        public struct CursorUIData
        {
            public PlayerCursorData.CursorGraphicType type;
            public GameObject CursorUIObject;
        }

        public List<CursorUIData> UISettings = new List<CursorUIData>();

        private void Start()
        {
            data.OnCursorTypeChangedEvent += OnCursorChange;
        }

        private void OnCursorChange(PlayerCursorData.CursorGraphicType type)
        {
            foreach (var setting in UISettings)
            {
                setting.CursorUIObject.SetActive(setting.type == type);
            }
        }

        private void OnDestroy()
        {
            data.OnCursorTypeChangedEvent -= OnCursorChange;
        }
    }
}