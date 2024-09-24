using System;
using UnityEngine;

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
            GridSystemVisual.instance.HideAllGridPositions();
            GridSystemVisual.instance.ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
        }
    }
}
