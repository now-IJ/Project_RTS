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

        public class OnShootEventArgs : EventArgs
        {
            public Unit targetUnit;
            public Unit shootingUnit;
        }
        
        private int maxShootDistance = 7;
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
            targetUnit.Damage();
        }
        
        public override string GetActionName()
        {
            return "Shoot";
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            ActionStart(OnActionComplete);

            targetUnit = LevelGrid.instance.GetUnitOnGridPosition(gridPosition);

            canShootBullet = true;
            state =  State.Aiming;
            float aimingStateTime = 1;
            stateTimer = aimingStateTime;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();

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
                    
                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
    }
}