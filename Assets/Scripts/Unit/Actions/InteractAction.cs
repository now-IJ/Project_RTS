using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class InteractAction : BaseAction
    {
        [SerializeField] private int maxInteractDistance = 1;
        
        public override string GetActionName()
        {
            return "Interact";
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(gridPosition);
            interactable.Interact(OnActionComplete);
            
            ActionStart(OnActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();
            
            for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
            {
                for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    IInteractable interactable = LevelGrid.instance.GetInteractableAtGridPosition(testGridPosition);
                    if (interactable == null)
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
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }

        private void OnInteractComplete()
        {
            ActionComplete();
        }
    }
}