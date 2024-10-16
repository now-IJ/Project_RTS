using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RS
{
    public class MoveAction : BaseAction
    {
        private List<Vector3> targetPositionList;
        private int currentPositionIndex;

        public event EventHandler ON_START_MOVING; 
        public event EventHandler ON_STOP_MOVING; 
        
        [Header("Movement Variables")] [SerializeField]
        private float movementSpeed = 4f;

        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float reachTargetAcceptance = 0.1f;
        [SerializeField] private int maxMoveDistance = 4;

        private void Update()
        {
            if (isActive)
            {
                HandleMovement();
            }
        }

        public void HandleMovement()
        {
            Vector3 targetPosition = targetPositionList[currentPositionIndex];
            float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);
            Vector3 moveDirection = targetPosition - transform.position;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

            if (distanceToTargetPosition >= reachTargetAcceptance)
            {
                moveDirection.Normalize();
                transform.position += moveDirection * movementSpeed * Time.deltaTime;
            }
            else
            {
                currentPositionIndex++;
                if (currentPositionIndex >= targetPositionList.Count)
                {
                    if (ON_STOP_MOVING != null)
                    {
                        ON_STOP_MOVING(this, EventArgs.Empty);
                    }

                    ActionComplete();
                }
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            List<GridPosition> pathGridPositions = Pathfinding.instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);
            
            currentPositionIndex = 0;
            targetPositionList = new List<Vector3>();

            foreach (GridPosition pathGridPosition in pathGridPositions)
            {
                targetPositionList.Add(LevelGrid.instance.GetWorldPosition(pathGridPosition));
            }
            
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

                    if (!Pathfinding.instance.IsWalkableGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    if (!Pathfinding.instance.HasPath(unit.GetGridPosition(), testGridPosition))
                    {
                        continue;
                    }

                    int pathfindingDistanceMultiplier = 10;
                    if (Pathfinding.instance.GetPathLength(unit.GetGridPosition(), testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
                    {
                        continue;
                    }

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtPosition * 10,
            };
        }

        public override string GetActionName()
        {
            return "Move";
        }
    }
}