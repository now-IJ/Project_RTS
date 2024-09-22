using System;
using UnityEngine;

public class UnitActionSelection : MonoBehaviour
{
    public static UnitActionSelection instance { get; private set; }
    
    public event EventHandler ON_SELECTED_UNIT_CHANGED;
    
    private Unit selectedUnit;

    [SerializeField] private LayerMask unitsLayer;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleUnitSelection();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && selectedUnit)
        {
            selectedUnit.MoveToPosition(MouseWorld.GetMouseHitPosition());
        }
    }

    private void HandleUnitSelection()
    {
        RaycastHit hit;
        Ray cameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraHitRay, out hit, float.MaxValue, unitsLayer))
        {
            if (hit.collider.gameObject.TryGetComponent<Unit>(out Unit clickedOnUnit))
            {
                SetSelectedUnit(clickedOnUnit);
            }
        }
    }

    private void SetSelectedUnit(Unit clickedOnUnit)
    {
        selectedUnit = clickedOnUnit;

        if (ON_SELECTED_UNIT_CHANGED != null)
        {
            ON_SELECTED_UNIT_CHANGED(this, EventArgs.Empty);
        }
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
