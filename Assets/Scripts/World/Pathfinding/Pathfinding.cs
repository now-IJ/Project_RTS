using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace RS
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding instance;
        
        private const int MOVE_STRAIGHT_COST = 10;  // straight distance 1 * 10
        private const int MOVE_DIAGONAL_COST = 14;  // diagonal distance a.sqr + b.sqr = c.sqr for a and b = 1 => c = 1.4 * 10 = 14
        
        [SerializeField] private GameObject gridDebugGameObjectPrefab;
        [SerializeField] private LayerMask obstacleLayerMask;
        
        private int width;
        private int height;
        private float cellSize;
        private GridSystem<PathNode> gridSystem;

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
            gridSystem.CreateDebugObjects(gridDebugGameObjectPrefab);
        }

        public void Setup(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            
            gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < this.height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
                    float rayCastOffsetDistance = 5f;
                    if (Physics.Raycast(
                            worldPosition + Vector3.down * rayCastOffsetDistance, Vector3.up, rayCastOffsetDistance * 2,
                            obstacleLayerMask))
                    {
                        GetNode(x,z).SetIsWalkable(false);
                    }
                }
            }
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            PathNode startNode = gridSystem.GetGridObject(startGridPosition);
            PathNode endNode = gridSystem.GetGridObject(endGridPosition);
            openList.Add(startNode);

            for (int x = 0; x < gridSystem.GetWidth(); x++)
            {
                for (int z = 0; z < gridSystem.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                    pathNode.SetGCost(int.MaxValue);
                    pathNode.SetHCost(0);
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
            }
            
            startNode.SetGCost(0);
            startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbour in GetNeighbourList(currentNode))
                {
                    if(closedList.Contains(neighbour))
                        continue;

                    if (!neighbour.GetIsWalkable())
                    {
                        closedList.Add(neighbour);
                        continue;
                    }

                    int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbour.GetGridPosition());
                    if (tentativeGCost < neighbour.GetGCost())
                    {
                        neighbour.SetCameFromPathNode(currentNode);
                        neighbour.SetGCost(tentativeGCost);
                        neighbour.SetHCost(CalculateDistance(neighbour.GetGridPosition(), endGridPosition));
                        neighbour.CalculateFCost();

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
            // NO PATH FOUND
            return null;
        }

        public int CalculateDistance(GridPosition a, GridPosition b)
        {
            GridPosition gridPositionDistance = a - b;
            int xDistance = Mathf.Abs(gridPositionDistance.x);
            int zDistance = Mathf.Abs(gridPositionDistance.z);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodes)
        {
            PathNode lowestFCostPathNode = pathNodes[0];
            for (int i = 0; i < pathNodes.Count; i++)
            {
                if (pathNodes[i].GetFCost() < lowestFCostPathNode.GetFCost())
                {
                    lowestFCostPathNode = pathNodes[i];
                }
            }

            return lowestFCostPathNode;
        }

        private PathNode GetNode(int x, int z)
        {
            return gridSystem.GetGridObject(new GridPosition(x, z));
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            GridPosition gridPosition = currentNode.GetGridPosition();

            if (gridPosition.x - 1 >= 0)
            {
                // Left
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    // Left Up
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }

                if (gridPosition.z - 1 >= 0)
                {
                    // Left Down
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }
            }

            if (gridPosition.x + 1 <= gridSystem.GetWidth())
            {

                // Right
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

                if (gridPosition.z + 1 < gridSystem.GetHeight())
                {
                    // Right Up
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }

                if (gridPosition.z - 1 >= 0)
                {
                    // Right Down
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                // Up
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                // Down
                neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
            }

            return neighbourList;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodes = new List<PathNode>();
            pathNodes.Add(endNode);

            PathNode currentNode = endNode;
            while (currentNode.GetCameFromPathNode() != null)
            {
                pathNodes.Add(currentNode.GetCameFromPathNode());
                currentNode = currentNode.GetCameFromPathNode();
            }
            
            pathNodes.Reverse();
            List<GridPosition> gridPositions = new List<GridPosition>();
            foreach (PathNode pathNode in pathNodes)
            {
                gridPositions.Add(pathNode.GetGridPosition());
            }

            return gridPositions;
        }
    }
}