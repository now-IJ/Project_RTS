using UnityEngine;

namespace RS
{
    public class PathNode
    {
        private GridPosition gridPosition;
        private int gCost;
        private int hCost;
        private int fCost;
        private PathNode cameFromPathNode;
        
        public PathNode(GridPosition gridPosition)
        {
            this.gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return gridPosition.ToString();
        }
    }
}
