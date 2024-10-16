using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class SpinAction : BaseAction
    {
        private float totalSpin;

        private void Update()
        {
            if (isActive)
            {
                float spinAddAmount = 360f * Time.deltaTime;
                transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
                totalSpin += spinAddAmount;
                if (totalSpin >= 360)
                {
                    ActionComplete();
                }
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
        {
            totalSpin = 0;
            ActionStart(OnActionComplete);
        }

        public override string GetActionName()
        {
            return "Spin";
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = unit.GetGridPosition();
            return new List<GridPosition> { unitGridPosition };
        }

        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0,
            };
        }

        public override int GetActionPointsCost()
        {
            return 2;
        }
    }
}