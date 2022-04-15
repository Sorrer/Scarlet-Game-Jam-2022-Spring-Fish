using UnityEngine;

namespace Game.Systems.Inventory
{

    public interface InventoryItem
    {
        public string GetID();
    }
    
    [CreateAssetMenu(fileName = "Inventory", menuName = "Game/Inventory")]
    public class InventorySO : ScriptableObject
    {
        //Stores all items
        //Game will have infinite amount if items, but each item will store up to a certain amount
        
    }
}
