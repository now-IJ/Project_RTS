using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class ShootAction : BaseAction
    {
        private enum State
        {
            Aiming,
            Shooting,
            Finish,
        }

        public event EventHandler<OnShootEventArgs> ON_SHOOT;
        public static event EventHandler<OnShootEventArgs> ON_ANY_SHOOT;

        public class OnShootEventArgs : EventArgs
        {
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        
        [SerializeField] private int maxShootDistance = 7;
        [SerializeField] private int shootDamage = 40;
        [SerializeField] private LayerMask obstacleLayerMask;
        private State state;
        private float stateTimer;
        private Unit targetUnit;
        private bool canShootBullet;
        
        private void Update()
        {
            if (!isActive)
                return;

            stateTimer -= Time.deltaTime;
            switch (state)
            {
                case State.Aiming:
                    Vector3 aimDirection = targetUnit.GetWorldPosition() - unit.GetWorldPosition();
                    aimDirection.Normalize();
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                    break;
                case State.Shooting:
                    if (canShootBullet)
                    {
                        canShootBullet = false;
                        Shoot();
                    }
                    break;
                case State.Finish:
                    
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
                case State.Aiming:
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    state = State.Finish;
                    float finishStateTime = 0.5f;
                    stateTimer = finishStateTime;
                    break;
                case State.Finish:
                    ActionComplete();
                    break;
                default:
                    break;
            }
        }

        private void Shoot()
        {
            if (ON_SHOOT != null)
            {
                ON_SHOOT(this, new OnShootEventArgs{targetUnit = targetUnit, shootingUnit = unit});
            }

            if (ON_ANY_SHOOT != null)
            {
                ON_ANY_SHOOT(this, new OnShootEventArgs { targetUnit = targetUnit, shootingUnit = unit });
            }
            targetUnit.Damage(shootDamage);
        }
        
        public override string GetActionName()
        {
            return "Shoot";
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);

            canShootBullet = true;
            state =  State.Aiming;
            float aimingStateTime = 1;
            stateTimer = aimingStateTime;
            
            ActionStart(OnActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition gridPosition = unit.GetGridPosition();
            return GetValidActionGridPositionList(gridPosition);
        }

        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -maxShootDistance; x <= maxShootDistance; x++)
            {
                for (int z = -maxShootDistance; z <= maxShootDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                    {
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > maxShootDistance)
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
                    
                    float unitShoulderHeigth = 1.5f;
                    Vector3 unitWorldPosition = LevelGrid.instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDirection = targetUnit.GetWorldPosition() - unitWorldPosition;
                    shootDirection.Normalize();
                    if (Physics.Raycast(
                            unitWorldPosition + Vector3.up * unitShoulderHeigth,
                            shootDirection,
                            Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                            obstacleLayerMask))
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
            Unit targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
            };
        
        }

        public Unit GetTargetUnit()
        {
            return targetUnit;
        }

        public int GetMaxShootDistance()
        {
            return maxShootDistance;
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}