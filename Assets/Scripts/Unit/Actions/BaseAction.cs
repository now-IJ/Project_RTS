using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS{
    public abstract class BaseAction : MonoBehaviour
    {
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
        }

        protected void ActionComplete()
        {
            isActive = false;
            OnActionComplete();
        }
    }
}
