using System;
using System.Collections.Generic;
using UnityEngine;

namespace RS
{
    public class GridTest : MonoBehaviour
    {

        [SerializeField] private Unit unit;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetMouseHitPosition());
                GridPosition startGridPosition = new GridPosition(0, 0);
                List<GridPosition> gridPositions = Pathfinding.instance.FindPath(startGridPosition, mouseGridPosition);

                for (int i = 0; i < gridPositions.Count - 1; i++)
                {
                    Debug.DrawLine(LevelGrid.instance.GetWorldPosition(gridPositions[i]), LevelGrid.instance.GetWorldPosition(gridPositions[i+1]), Color.white, 10f);
                }
            }
        }
    }
}