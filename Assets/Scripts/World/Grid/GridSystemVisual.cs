using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RS
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual instance;

        [SerializeField] private GameObject gridSystemVisualPrefab;
        [SerializeField] private List<GridVisualMaterial> gridVisualMaterials = new List<GridVisualMaterial>();

        [Serializable]
        public struct GridVisualMaterial
        {
            public GridVisualColour gridVisualColour;
            public Material material;
        }
        public enum GridVisualColour
        {
            White,
            Blue,
            Red,
            Orange,
            Green,
        }

        private GridSystemVisualSingle[,] gridSystemVisualSinglesArray;

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
        }

        private void Start()
        {
            gridSystemVisualSinglesArray =
                new GridSystemVisualSingle[LevelGrid.instance.GetWidth(), LevelGrid.instance.GetHeight()];

            for (int x = 0; x < LevelGrid.instance.GetWidth(); x++)
            {
                for (int z = 0; z < LevelGrid.instance.GetHeight(); z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);

                    GameObject gridSystemVisualSingleObject = Instantiate(gridSystemVisualPrefab,
                        LevelGrid.instance.GetWorldPosition(gridPosition), quaternion.identity);

                    gridSystemVisualSinglesArray[x, z] =
                        gridSystemVisualSingleObject.GetComponent<GridSystemVisualSingle>();
                }
            }

            UnitActionSystem.instance.ON_SELECTED_ACTION_CHANGED += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.instance.ON_ANY_UNIT_MOVED += LevelGrid_OnAnyUnitMoved;
            UpdateGridVisual();
        }


        public void HideAllGridPositions()
        {
            foreach (GridSystemVisualSingle gridSystemVisual in gridSystemVisualSinglesArray)
            {
                gridSystemVisual.Hide();
            }
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualColour colour)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                { 
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                    
                    if(!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                        continue;
                    
                    int distance = Mathf.Abs(x) + Mathf.Abs(z);
                    if(distance > range)
                       continue;
                   
                    gridPositions.Add(testGridPosition);
                }
                
            }
            
            ShowGridPositionList(gridPositions, GridVisualColour.Orange);
        }
        
        private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualColour colour)
        {
            List<GridPosition> gridPositions = new List<GridPosition>();
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                { 
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                    
                    if(!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                        continue;
              
                    gridPositions.Add(testGridPosition);
                }
                
            }
            
            ShowGridPositionList(gridPositions, GridVisualColour.Orange);
        }

        public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualColour colour)
        {
            foreach (GridPosition gridPosition in gridPositions)
            {
                gridSystemVisualSinglesArray[gridPosition.x, gridPosition.z].Show(GetGridVisualColourMaterial(colour));
            }
        }

        private void UpdateGridVisual()
        {
            HideAllGridPositions();

            Unit selectedUnit = UnitActionSystem.instance.GetSelectedUnit();
            BaseAction selectedAction = UnitActionSystem.instance.GetSelecetedAction();

            GridVisualColour gridVisualColour;
            switch (selectedAction)
            {
                case MoveAction moveAction:
                    gridVisualColour = GridVisualColour.Green;
                    break;
                case ShootAction shootAction:
                    gridVisualColour = GridVisualColour.Red;
                    ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualColour.Orange);
                    break;
                case SpinAction spinAction:
                    gridVisualColour = GridVisualColour.Blue;
                    break;
                case GrenadeAction grenadeAction:
                    gridVisualColour = GridVisualColour.White;
                    break;
                case SwordAction swordAction:
                    gridVisualColour = GridVisualColour.Red;
                    ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSlashDistance(), GridVisualColour.Orange);
                    break;
                case InteractAction interactAction:
                    gridVisualColour = GridVisualColour.Blue;
                    break;
                default:
                    gridVisualColour = GridVisualColour.White;
                    break;
            }
            
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualColour);
        }
        
        
        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateGridVisual();   
        }
        
        
        private void LevelGrid_OnAnyUnitMoved(object sender, EventArgs e)
        {
            UpdateGridVisual();  
        }

        private Material GetGridVisualColourMaterial(GridVisualColour colour)
        {
            foreach (GridVisualMaterial materialColour in gridVisualMaterials)
            {
                if (materialColour.gridVisualColour == colour)
                {
                    return materialColour.material;
                }
            }
            return null;
        }
    }
}