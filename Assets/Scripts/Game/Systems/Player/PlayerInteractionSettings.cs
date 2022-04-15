using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Systems.Player
{
    [CreateAssetMenu(fileName = "Player Interaction Settings", menuName = "Game/InteractionSettings")]
    public class PlayerInteractionSettings : ScriptableObject
    {
        public bool EnableInteractions;
        public bool SingleInteraction;
        public LayerMask InteractionLayer;
        public bool AutoChangeCursor = true;
        [FormerlySerializedAs("CursorType")] public PlayerCursorData.CursorGraphicType cursorGraphicType;
        public PlayerCursorData.CursorGraphicType OnInteractType;
        
        
        
    }
}