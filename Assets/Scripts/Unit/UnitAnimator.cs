using System;
using UnityEngine;

namespace RS
{
    public class UnitAnimator : MonoBehaviour
    {
        private Animator animator;
        private MoveAction moveAction;
        private ShootAction shootAction;
        private SwordAction swordAction;
        
        [Header("Rifle")]
        [SerializeField] private GameObject rifle;
        [SerializeField] private GameObject bulletProjectilePrefab;
        [SerializeField] private Transform bulletSpawnPoint;

        [Header("Sword")]
        [SerializeField] private GameObject sword;
        
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            moveAction = GetComponent<MoveAction>();
            shootAction = GetComponent<ShootAction>();
            swordAction = GetComponent<SwordAction>();
        }

        private void Start()
        {
            if (moveAction != null)
            {
                moveAction.ON_START_MOVING += MoveAction_OnStartMoving;
                moveAction.ON_STOP_MOVING += MoveAction_OnStopMoving;
            }

            if (shootAction != null)
            {
                shootAction.ON_SHOOT += ShootAction_OnShoot;
            }

            if (swordAction != null)
            {
                swordAction.ON_SWORD_SLASH_START += SwordAction_OnSwordSlashStart;
                swordAction.ON_SWORD_SLASH_END += SwordAction_OnSwordSlashEnd;
            }
            
            EquipRifle();
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
        
        private void SwordAction_OnSwordSlashStart(object sender, EventArgs e)
        {
            EquipSword();
            animator.SetTrigger("SwordSlash");
        }
        
        private void SwordAction_OnSwordSlashEnd(object sender, EventArgs e)
        {
            EquipRifle();
        }

        private void EquipSword()
        {
            rifle.SetActive(false);
            sword.SetActive(true);
        }

        private void EquipRifle()
        {
            sword.SetActive(false);
            rifle.SetActive(true);
        }
    }
}