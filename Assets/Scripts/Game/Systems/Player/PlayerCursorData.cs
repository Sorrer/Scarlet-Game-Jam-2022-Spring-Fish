using System;
using UnityEngine;

namespace Game.Systems.Player
{
    [CreateAssetMenu(fileName = "Player Cursor Data", menuName = "Game/Data/Cursor")]
    public class PlayerCursorData : ScriptableObject
    {

        public enum CursorGraphicType
        {
            DEFAULT, BUILD, PICKUP, INVALID, SELECT, SELECT_CIRCLE_INVALID, SELECT_CIRCLE, FEED, EYE, EYE_CLOSED
        }
        
        
        [SerializeField]
        [Tooltip("DEBUG ONLY - CHANGING VALUES DO NOTHING")]
        private CursorGraphicType _type;

        public delegate void OnCursorTypeChanged(CursorGraphicType newGraphicType);

        public OnCursorTypeChanged OnCursorTypeChangedEvent;
        
        public CursorGraphicType cursorGraphicType
        {
            set
            {
                OnCursorTypeChangedEvent?.Invoke(value);
                _type = value;
            }
            get
            {
                return _type;
            }
        }
        
        public Vector3 position;

    }
}