using System;
using Unity.Mathematics;
using UnityEngine;

namespace RS
{
    public class GrenadeProjectile : MonoBehaviour
    {
        public static event EventHandler ON_ANY_GRENADE_EXPLODED; 
        
        [SerializeField] private float moveSpeed = 15f;
        [SerializeField] private float damageRadius = 2;
        [SerializeField] private float reachedTargetDistance = 0.2f;
        [SerializeField] private GameObject explosionVFX;
        [SerializeField] private AnimationCurve yArcAnimationCurve;
        private Vector3 targetPosition;
        private Action OnGrenadeBehaviourComplete;
        private TrailRenderer trailRenderer;
        private float totalDistance;
        private Vector3 xzPosition;

        private void Awake()
        {
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        private void Update()
        {
            Vector3 moveDirection = targetPosition - xzPosition;
            moveDirection.Normalize();
            xzPosition += moveDirection * moveSpeed * Time.deltaTime;
            float distance = Vector3.Distance(xzPosition, targetPosition);
            float distanceNormalized = 1 - (distance / totalDistance);
            float yPosition = yArcAnimationCurve.Evaluate(distanceNormalized);
            transform.position = new Vector3(xzPosition.x, yPosition, xzPosition.z);
            
            if (Vector3.Distance(xzPosition, targetPosition) < reachedTargetDistance)
            {
                Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                    {
                        targetUnit.Damage(30);
                    }
                }

                if (ON_ANY_GRENADE_EXPLODED != null)
                {
                    ON_ANY_GRENADE_EXPLODED(this, EventArgs.Empty);
                }

                trailRenderer.transform.parent = null;
                Instantiate(explosionVFX, targetPosition + Vector3.up, quaternion.identity);
                Destroy(gameObject);
                OnGrenadeBehaviourComplete();
            }
        }

        public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviourComplete)
        {
            this.OnGrenadeBehaviourComplete = OnGrenadeBehaviourComplete;
            targetPosition = LevelGrid.instance.GetWorldPosition(targetGridPosition);

            xzPosition = transform.position;
            xzPosition.y = 0;
            totalDistance = Vector3.Distance(transform.position, targetPosition);
        }
    }
}