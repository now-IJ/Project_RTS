using System;
using UnityEngine;

namespace RS
{
    public class UnitSelectVisual : MonoBehaviour
    {
        private Unit unit;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            meshRenderer.enabled = false;
            UnitActionSystem.instance.ON_SELECTED_UNIT_CHANGED += UnitActionSelected_OnSelectedUnitChanged;
        }

        private void UnitActionSelected_OnSelectedUnitChanged(object sender, EventArgs empty)
        {
            if (UnitActionSystem.instance.GetSelectedUnit() == unit)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }

        private void OnDestroy()
        {
            UnitActionSystem.instance.ON_SELECTED_UNIT_CHANGED -= UnitActionSelected_OnSelectedUnitChanged;
        }
    }
}