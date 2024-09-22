using System;
using UnityEngine;

public class UnitSelectVisual : MonoBehaviour
{
    private UnitBehaviour unitBehaviour;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        unitBehaviour = GetComponentInParent<UnitBehaviour>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        meshRenderer.enabled = false;
        UnitActionSelection.instance.ON_SELECTED_UNIT_CHANGED += UnitActionSelected_OnSelectedUnitChanged;
    }

    private void UnitActionSelected_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        if (UnitActionSelection.instance.GetSelectedUnit() == unitBehaviour)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
