using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RS{
    public abstract class BaseAction : MonoBehaviour
    {
        public static event EventHandler ON_ANY_ACTION_START;
        public static event EventHandler ON_ANY_ACTION_COMPLETED;
        
        protected Action OnActionComplete;
        protected bool isActive;
        protected Unit unit;

        protected virtual void Awake()
        {
            unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void TakeAction(GridPosition gridPosition, Action OnActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
        {
            List<GridPosition> validActionGridPositions = GetValidActionGridPositionList();
            return validActionGridPositions.Contains(gridPosition);
        }

        public virtual int GetActionPointsCost()
        {
            return 1;
        }

        public abstract List<GridPosition> GetValidActionGridPositionList();

        protected void ActionStart(Action OnActionComplete)
        {
            isActive = true;
            this.OnActionComplete = OnActionComplete;
            
            if (ON_ANY_ACTION_START != null)
            {
                ON_ANY_ACTION_START(this, EventArgs.Empty);
            }
        }

        protected void ActionComplete()
        {
            
            if (ON_ANY_ACTION_COMPLETED != null)
            {
                ON_ANY_ACTION_COMPLETED(this, EventArgs.Empty);
            }
            isActive = false;
            OnActionComplete();
        }

        public Unit GetUnit()
        {
            return unit;
        }
        
        public EnemyAIAction GetBestEnemyAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count > 0)
            {
                enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
                return enemyAIActionList[0];
            } else
            {
                // No possible Enemy AI Actions
                return null;
            }

        }

        public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    }
}
