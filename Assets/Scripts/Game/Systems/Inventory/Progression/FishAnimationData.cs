using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Inventory.Progression
{
    public class FishAnimationData : MonoBehaviour
    {
        [Header("Ordered head to tail")]
        public List<GameObject> bones = new List<GameObject>();
        
        public List<Vector3> positions = new List<Vector3>();

        private int currentLocation = 0;

        public bool IsFinished = false;

        public float StoppingRadius;

        public float FishWave;

        public float Speed;

        public Vector3 Velocity;

        public float MaxSpeed;
        
        public void StartFish()
        {
            this.transform.position = positions[0];

            currentLocation = 1;
            IsFinished = false;
        }
        
        private void Update()
        {
            if (IsFinished) return;
            
            this.transform.LookAt(positions[currentLocation]);

            Velocity += Speed * Time.deltaTime * (positions[currentLocation] -
                        this.transform.position).normalized;
            
            Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);


            this.transform.position += Velocity * Time.deltaTime;

            if (Vector3.Distance(this.transform.position, positions[currentLocation]) <= StoppingRadius)
            {
                currentLocation++;
            }

            if (currentLocation > positions.Count)
            {
                IsFinished = true;
            }

        }
    }
}