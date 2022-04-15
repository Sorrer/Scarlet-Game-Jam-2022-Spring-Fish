using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class FakeBouncy : MonoBehaviour
    {

        public float Height;
        public float Gravity;
        public float WaterForce;
        public float Dampening;

        public float maxSpeed;
        public float SurfaceWaveHeight;
        public float WaveSpeed;

        public float Deadzone;
        private float momentum;

        private float currentWaveHeight;

        private bool currentWaveMovement;
        // Start is called before the first frame update
        void Start()
        {
            currentWaveHeight = SurfaceWaveHeight * Random.value;
            currentWaveMovement = Random.value > 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            float waveHeight = Height + currentWaveHeight;

            if (currentWaveHeight > SurfaceWaveHeight)
            {
                currentWaveHeight = SurfaceWaveHeight;
                currentWaveMovement = false;
            }

            if (currentWaveHeight < 0)
            {
                currentWaveHeight = SurfaceWaveHeight;
                currentWaveMovement = true;
            }

            currentWaveHeight += currentWaveMovement ? WaveSpeed * Time.deltaTime : -WaveSpeed * Time.deltaTime;

            if (this.transform.position.y > waveHeight)
            {
                momentum += Gravity * Time.deltaTime;
            }
            else
            {
                momentum -= Gravity * Time.deltaTime;
            }

            momentum -= momentum * Dampening;

            momentum = Mathf.Clamp(momentum, -maxSpeed, maxSpeed);
            
            if(Mathf.Abs(momentum) >  Deadzone)
                this.transform.position = this.transform.position + Vector3.up * (momentum * Time.deltaTime);
        }
    }
}
