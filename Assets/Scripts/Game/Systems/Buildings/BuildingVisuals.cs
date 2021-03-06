using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingVisuals : MonoBehaviour
{
    [SerializeField] GameObject defaultObj;
    GameObject defaultDup; // holds generated instance of defaultObj duplicate
    public enum EffectSelect {
        Activate, // 0
        Hover, // 1
        Unhover, // 2
        Confirm, // 3
        Finish //4
    }
    [SerializeField] Material blueTranslucent, greenTranslucent, highlight;
    [SerializeField] ParticleSystem particles;
    // Camera shake effect left off for now...
    bool isVisible;
    BoxCollider collider;
    Vector3 particleCenter;
    float colliderBottom, colliderSize;

    public void Awake() {
        defaultDup = (GameObject) Instantiate(defaultObj);
        defaultDup.transform.position = this.gameObject.transform.position;
        defaultDup.transform.rotation = this.gameObject.transform.rotation;
        defaultDup.transform.localScale = this.gameObject.transform.localScale;
        
        defaultDup.SetActive(false);
        isVisible = true;

        collider = this.gameObject.GetComponent<BoxCollider>();
    }

    public void visualsControl(EffectSelect effect) {
        switch (effect) {
            case EffectSelect.Activate:
                assignMaterial(this.gameObject, blueTranslucent);
                break;
            case EffectSelect.Hover:
                assignMaterial(this.gameObject, highlight);
                break;
            case EffectSelect.Unhover:
                assignMaterial(this.gameObject, blueTranslucent);
                break;
            case EffectSelect.Confirm:
                assignMaterial(this.gameObject, greenTranslucent);
                break;
            case EffectSelect.Finish:
                // "Restores" default material by making clone active
                defaultMaterial();
                setDown(this.gameObject);
                break;
        }
    }

    void Update() {
        #if UNITY_EDITOR
        testFX();
        #endif
    }

    private void assignMaterial(GameObject obj, Material material) {
        if (isVisible == false) {
            setVisibility(this.gameObject, "vis");
            isVisible = true;
        }

        if (obj == null) return;

        var renderer = obj.GetComponent<MeshRenderer>();
        if(renderer != null) {
            renderer.material = material;
            
            var materials = renderer.materials;

            for (int i = 0; i < renderer.materials.Length; i++) {
                materials[i] = material;
            }
            renderer.materials = materials;
        }

        foreach(Transform child in obj.transform) {
            assignMaterial(child.gameObject, material);
        }
    }

    private void defaultMaterial() {
        defaultDup.SetActive(true);
        setVisibility(this.gameObject, "invis");
        isVisible = false;
    }
    private void setVisibility(GameObject obj, string visibility){
        // Sets all cihldren of a gameobj to inactive --> invis
        if (obj == null) return;
        switch (visibility) {
            case "invis":
                foreach(Transform child in obj.transform) {
                    child.gameObject.SetActive(false);
                }
                break;
            case "vis":
                foreach(Transform child in obj.transform) {
                    child.gameObject.SetActive(true);
                }
                break;
        }
    }

    public void setDown(GameObject obj) {
        particleCenter = collider.bounds.center;
        colliderBottom = collider.bounds.center.y - collider.bounds.size.y;
        particleCenter.y = colliderBottom;

        colliderSize = collider.bounds.size.x;

        Instantiate(particles, obj.transform.position, Quaternion.identity);
    }

    public void testFX() {
        if (Input.GetKeyDown(KeyCode.O)) {
            Debug.Log("Yuh partcles");
            setDown(this.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            visualsControl(EffectSelect.Activate);
        }
         if (Input.GetKeyDown(KeyCode.J)) {
            visualsControl(EffectSelect.Hover);
        }
         if (Input.GetKeyDown(KeyCode.K)) {
            visualsControl(EffectSelect.Unhover);
        }
         if (Input.GetKeyDown(KeyCode.L)) {
            visualsControl(EffectSelect.Confirm);
        }
         if (Input.GetKeyDown(KeyCode.M)) {
            visualsControl(EffectSelect.Finish);
        }
    }
}
