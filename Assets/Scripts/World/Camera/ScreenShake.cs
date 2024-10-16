using System;
using Cinemachine;
using UnityEngine;

namespace RS
{
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake instance;
        
        private CinemachineImpulseSource cinemachineImpulseSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Shake(float intensity = 1f)
        {
            cinemachineImpulseSource.GenerateImpulse(intensity);  
        }
    }
}