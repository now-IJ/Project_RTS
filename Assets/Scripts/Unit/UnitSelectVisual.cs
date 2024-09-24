using System;
using UnityEngine;

public class UnitSelectVisual : MonoBehaviour
{
    private Unit _unit;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        _unit = GetComponentInParent<Unit>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.enabled = false;
        UnitActionSystem.instance.ON_SELECTED_UNIT_CHANGED += UnitActionSelected_OnSelectedUnitChanged;
    }

    private void UnitActionSelected_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        if (UnitActionSystem.instance.GetSelectedUnit() == _unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
