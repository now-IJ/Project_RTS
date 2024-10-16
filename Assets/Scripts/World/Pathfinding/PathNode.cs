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

        public void SetGCost(int gCost)
        {
            this.gCost = gCost;
        }
        
        public int GetGCost()
        {
            return gCost;
        }
        
        public void SetHCost(int hCost)
        {
            this.hCost = hCost;
        }
        
        public int GetHCost()
        {
            return hCost;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        
        public int GetFCost()
        {
            return fCost;
        }

        public void ResetCameFromPathNode()
        {
            cameFromPathNode = null;
        }

        public void SetCameFromPathNode(PathNode pathNode)
        {
            cameFromPathNode = pathNode;
        }
        
        public PathNode GetCameFromPathNode()
        {
            return cameFromPathNode;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }
    }
}
