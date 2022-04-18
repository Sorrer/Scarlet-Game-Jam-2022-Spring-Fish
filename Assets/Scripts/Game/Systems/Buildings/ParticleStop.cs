using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStop : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem particles;
    //[SerializeField] float size;
    private IEnumerator coroutineDestroy;
    void Awake()
    {
        this.gameObject.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
        //this.gameObject.transform.localScale = new Vector3(size, size, size);

        // Destroys particle system after 2 seconds
        coroutineDestroy = destroyParticles(particles);
        StartCoroutine(coroutineDestroy);
    }

    private IEnumerator destroyParticles(ParticleSystem sys) {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
