using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class LevelGrid : MonoBehaviour
    {
        public event EventHandler ON_ANY_UNIT_MOVED;
        
        public static LevelGrid instance { get; private set; }

        private GridSystem<GridObject> gridSystem;

        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private float cellSize = 2f;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        }

        private void Start()
        {
            Pathfinding.instance.Setup(width, height, cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.AddUnit(unit);
        }

        public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
        {
            return gridSystem.GetGridObject(gridPosition).GetUnitList();
        }

        public void ClearUnitAtGridPosition(GridPosition gridPosition, Unit unit)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.RemoveUnit(unit);
        }

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            ClearUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);

            if (ON_ANY_UNIT_MOVED != null)
            {
                ON_ANY_UNIT_MOVED(this, EventArgs.Empty);
            }
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return gridSystem.GetGridPosition(worldPosition);
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return gridSystem.GetWorldPosition(gridPosition);
        }

        public bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridSystem.isValidGridPosition(gridPosition);
        }

        public int GetWidth()
        {
            return gridSystem.GetWidth();
        }

        public int GetHeight()
        {
            return gridSystem.GetHeight();
        }

        public bool HasUnitOnGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            return gridObject.HasAnyUnit();
        }
        
        public Unit GetUnitOnGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            return gridObject.GetUnit();
        }

        public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            return gridObject.GetInteractable();
        }
        
        public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
        {
            GridObject gridObject = gridSystem.GetGridObject(gridPosition);
            gridObject.SetInteractable(interactable);
        }
        
    }
}