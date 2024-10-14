using System;
using UnityEngine;

namespace RS
{
    public class UnitAnimator : MonoBehaviour
    {
        private Animator animator;
        private MoveAction moveAction;
        private ShootAction shootAction;

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

        private void ShootAction_OnShoot(object sender, EventArgs e)
        {
            animator.SetTrigger("Shoot");            
        }
    }
}