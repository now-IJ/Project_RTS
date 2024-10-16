using System;
using System.Diagnostics;
using UnityEngine;

namespace RS 
{
    public class BulletProjectile : MonoBehaviour
    {
        private Vector3 targetPosition;
        private TrailRenderer trail;
        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private GameObject BulletHitVFX;
        private void Awake()
        {
            trail = GetComponentInChildren<TrailRenderer>();
        }

        public void SetUp(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        private void Update()
        {
            Vector3 moveDirection = targetPosition - transform.position;
            moveDirection.Normalize();

            float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
            
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = targetPosition;
                trail.transform.parent = null;
                Instantiate(BulletHitVFX, targetPosition, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    
}