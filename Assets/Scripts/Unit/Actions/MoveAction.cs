using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RS
{
    public class MoveAction : BaseAction
    {
        private Vector3 targetPosition;

        public event EventHandler ON_START_MOVING; 
        public event EventHandler ON_STOP_MOVING; 
        
        [Header("Movement Variables")] [SerializeField]
        private float movementSpeed = 4f;

        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float reachTargetAcceptance = 0.1f;
        [SerializeField] private int maxMoveDistance = 4;

        protected override void Awake()
        {
            base.Awake();
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
            }
            else
            {
                if (ON_STOP_MOVING != null)
                {
                    ON_STOP_MOVING(this, EventArgs.Empty);
                }
                ActionComplete();
            }

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            this.targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
            if (ON_START_MOVING != null)
            {
                ON_START_MOVING(this, EventArgs.Empty);
            }
            ActionStart(OnActionComplete);
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