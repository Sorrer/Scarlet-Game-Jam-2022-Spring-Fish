using Game.Systems.CursorInteractable;
using UnityEngine;
using UnityEngineInternal;

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

            go.name = "Interactable";

            go.GetComponent<Renderer>();
            
            
            goModel.name = "Visual";
            
            var collider = goModel.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            else
            {
                Debug.LogError("No collider on pick up object, this object wont be pickupable");
                var renderer = go.GetComponent<MeshFilter>();
                if (renderer != null)
                {
                    var meshC = go.AddComponent<MeshCollider>();
                    meshC.sharedMesh = renderer.mesh;
                }
                else
                {
                    var meshC = go.AddComponent<SphereCollider>();
                    meshC.radius = 2;
                }
            }

        }

        public void DisableRenderers(GameObject obj)
        {
            
        }
    }
}
