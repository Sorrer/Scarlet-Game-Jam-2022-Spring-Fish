using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingVisuals : MonoBehaviour
{
    public enum EffectSelect {
        Activate, // 0
        Hover, // 1
        Unhover, // 2
        Confirm, // 3
        Finish //4
    }
    public Material blueTranslucent, greenTranslucent, highlight;
    public ParticleSystem particles;
    // Camera shake effect left off for now...

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
                // Leave default object materials
                setDown(this.gameObject);
                break;
        }
    }

    void Update() {
        testSetDown();
    }

    private void assignMaterial(GameObject obj, Material material) {
        if (obj == null) return;
        
        this.GetComponent<MeshRenderer>().material = material;

        foreach(Transform child in obj.transform) {
            assignMaterial(child.gameObject, material);
        }
    }

    public void setDown(GameObject obj) {
        Instantiate(particles, obj.transform.position, Quaternion.identity);
    }

    public void testSetDown() {
        if (Input.GetKeyDown(KeyCode.O)) {
            Debug.Log("Yuh partcles");
            setDown(this.gameObject);
        }
    }
}
