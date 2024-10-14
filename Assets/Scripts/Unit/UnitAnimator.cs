using System;
using UnityEngine;

namespace RS
{
    public class UnitAnimator : MonoBehaviour
    {
        private Animator animator;
        private MoveAction moveAction;
        private ShootAction shootAction;
        [SerializeField] private GameObject bulletProjectilePrefab;
        [SerializeField] private Transform bulletSpawnPoint;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            moveAction = GetComponent<MoveAction>();
            shootAction = GetComponent<ShootAction>();
        }

        private void Start()
        {
            moveAction.ON_START_MOVING += MoveAction_OnStartMoving;
            moveAction.ON_STOP_MOVING += MoveAction_OnStopMoving;
            shootAction.ON_SHOOT += ShootAction_OnShoot;
        }

        private void MoveAction_OnStartMoving(object sender, EventArgs e)
        {
            animator.SetBool("IsWalking", true);
        }
        
        private void MoveAction_OnStopMoving(object sender, EventArgs e)
        {
            animator.SetBool("IsWalking", false);
        }

        private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            animator.SetTrigger("Shoot");
            GameObject bulletProjectileObject = Instantiate(bulletProjectilePrefab, bulletSpawnPoint.position, Quaternion.identity);
            BulletProjectile bulletProjectile = bulletProjectileObject.GetComponent<BulletProjectile>();
            Vector3 targetUnitHitPosition = e.targetUnit.GetWorldPosition();
            targetUnitHitPosition.y = bulletSpawnPoint.position.y;
            bulletProjectile.SetUp(targetUnitHitPosition);
        }
    }
}