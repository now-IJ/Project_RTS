using System;
using UnityEngine;


namespace RS{
    public class GridSystem<TGridObject>
    {
        private int width;
        private int height;
        private float cellSize;
        private bool isValdGridPosition = true;
        private TGridObject[,] gridObjectArray;

        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            gridObjectArray = new TGridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    gridObjectArray[x, z] = createGridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
        }

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition(
                Mathf.RoundToInt(worldPosition.x / cellSize),
                Mathf.RoundToInt(worldPosition.z / cellSize));
        }

        public TGridObject GetGridObject(GridPosition gridPosition)
        {
            return gridObjectArray[gridPosition.x, gridPosition.z];
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public bool isValidGridPosition(GridPosition gridPosition)
        {
            bool insideOfGrid = gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < height;
            return insideOfGrid && isValdGridPosition;
        }

        public void setValidGridPosition(GridPosition gridPosition, bool isValid)
        {
            isValdGridPosition = isValid;
        }
    }
}
