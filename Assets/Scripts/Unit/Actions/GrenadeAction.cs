using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class GrenadeAction : BaseAction
    {
        [SerializeField] private GameObject grenadePrefab;
        [SerializeField] private int maxThrowDistance = 7;
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }
        }

        public override string GetActionName()
        {
            return "Grenade";
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            GameObject grenade = Instantiate(grenadePrefab, unit.GetWorldPosition(), Quaternion.identity);
            grenade.GetComponent<GrenadeProjectile>().Setup(gridPosition, OnGrenadeBehaviourComplete);
            ActionStart(OnActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();
            
            for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
            {
                for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxThrowDistance)
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
            return new EnemyAIAction{gridPosition = gridPosition, actionValue = 0,};
        }

        private void OnGrenadeBehaviourComplete()
        {
            ActionComplete();
        }
    }
}