using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class SwordAction : BaseAction
    {
        public static event EventHandler ON_ANY_SWORD_HIT;

        public event EventHandler ON_SWORD_SLASH_START;
        public event EventHandler ON_SWORD_SLASH_END;
        private enum State
        {
            SwordSlashStart,
            SwordSlashEnd,
        }
        
        [SerializeField] private int maxSlashDistance = 1;
        private State state;
        private float stateTimer;
        private Unit targetUnit;
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }
           
            stateTimer -= Time.deltaTime;
            switch (state)
            {
                case State.SwordSlashStart:
                    Vector3 aimDirection = targetUnit.GetWorldPosition() - unit.GetWorldPosition();
                    aimDirection.Normalize();
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                    break;
                case State.SwordSlashEnd:
                
                    break;
                default:
                    break;
            }
            
            if (stateTimer <= 0)
            {
                NextState();
            }
        }
        
        private void NextState()
        {
            switch (state)
            {
                case State.SwordSlashStart:
                    state = State.SwordSlashEnd;
                    float finishStateTime = 0.5f;
                    stateTimer = finishStateTime;
                    targetUnit.Damage(100);
                    if (ON_ANY_SWORD_HIT != null)
                    {
                        ON_ANY_SWORD_HIT(this,EventArgs.Empty);
                    }
                    break;
                case State.SwordSlashEnd:
                    if (ON_SWORD_SLASH_END != null)
                    {
                        ON_SWORD_SLASH_END(this,EventArgs.Empty);
                    }
                    ActionComplete();
                    break;
                default:
                    break;
            }
        }

        public override string GetActionName()
        {
            return "Sword";
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);
            state = State.SwordSlashStart;
            float startStateTime = 0.7f;
            stateTimer = startStateTime;

            if (ON_SWORD_SLASH_START != null)
            {
                ON_SWORD_SLASH_START(this,EventArgs.Empty);
            }
            ActionStart(OnActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();
            
            for (int x = -maxSlashDistance; x <= maxSlashDistance; x++)
            {
                for (int z = -maxSlashDistance; z <= maxSlashDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }
                    
                    if (!LevelGrid.instance.HasUnitOnGridPosition(testGridPosition))
                    {
                        // Grid Position is empty
                        continue;
                    }
                    
                    Unit targetUnit = LevelGrid.instance.GetUnitOnGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy() == unit.IsEnemy())
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
                actionValue = 200,
            };
        }

        public int GetMaxSlashDistance()
        {
            return maxSlashDistance;
        }
    }
}