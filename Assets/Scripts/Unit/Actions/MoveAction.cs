using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RS
{
    public class MoveAction : BaseAction
    {
        private Animator animator;

        private Vector3 targetPosition;

        [Header("Movement Variables")] [SerializeField]
        private float movementSpeed = 4f;

        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float reachTargetAcceptance = 0.1f;
        [SerializeField] private int maxMoveDistance = 4;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
            targetPosition = transform.position;
        }

        private void Update()
        {
            if (isActive)
            {
                HandleMovement();
            }
        }

        public void HandleMovement()
        {

            float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);
            Vector3 moveDirection = targetPosition - transform.position;

            if (distanceToTargetPosition >= reachTargetAcceptance)
            {
                moveDirection.Normalize();
                transform.position += moveDirection * movementSpeed * Time.deltaTime;
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
                ActionComplete();
            }

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            ActionStart(OnActionComplete);
            this.targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();

            for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (unitGridPosition == testGridPosition)
                    {
                        continue;
                    }

                    if (LevelGrid.instance.HasUnitOnGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override string GetActionName()
        {
            return "Move";
        }
    }
}