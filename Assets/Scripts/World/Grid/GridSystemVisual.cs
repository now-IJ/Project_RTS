using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual instance;
    
    [SerializeField] private GameObject gridSystemVisualPrefab;

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
        
        for (int x = 0 ;x < LevelGrid.instance.GetWidth(); x++)
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
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        foreach (GridSystemVisualSingle gridSystemVisual in gridSystemVisualSinglesArray)
        {
            gridSystemVisual.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridSystemVisualSinglesArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();
        BaseAction selectedAction = UnitActionSystem.instance.GetSelecetedAction();
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
    }
}
