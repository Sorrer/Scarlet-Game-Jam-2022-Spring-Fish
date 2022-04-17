using Game.Systems.CursorInteractable;
using UnityEngine;

namespace Game.Systems.Inventory
{
    public class ItemSpawn : MonoBehaviour
    {
        public InventorySO inventory;

        public void SpawnPickUpObject(InventoryItem item)
        {
            var go = Instantiate(item.Prefab, this.transform);
            var goModel = Instantiate(item.Prefab, this.transform);
            go.transform.position = this.transform.position;
            goModel.transform.position = this.transform.position;
            
            var pickup = go.AddComponent<PickUpInteractable>();

            pickup.inventory = inventory;

            pickup.root = this.gameObject;
            pickup.itemReference = item;
            pickup.modelObject = goModel;

            go.layer = LayerMask.NameToLayer("Pickup");

        }
    }
}
