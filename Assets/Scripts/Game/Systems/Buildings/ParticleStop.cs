using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStop : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem particles;
    private IEnumerator coroutineDestroy;
    void Awake()
    {
        // Destroys particle system after 2 seconds
        coroutineDestroy = destroyParticles(particles);
        StartCoroutine(coroutineDestroy);
    }

    private IEnumerator destroyParticles(ParticleSystem sys) {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }
}
