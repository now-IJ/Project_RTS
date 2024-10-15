using System;
using UnityEngine;

namespace RS
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool invert;
        private Transform cameraTransform;

        private void Awake()
        {
            cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (invert)
            {
                Vector3 directionToCamera = (cameraTransform.position - transform.position);
                directionToCamera.Normalize();
                transform.LookAt(transform.position + directionToCamera * -1);
            }
            else
            {
                transform.LookAt(cameraTransform);
            }
        }
    }
}