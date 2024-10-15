using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RS
{
    public class UnitUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointsText;
        [SerializeField] private Image healthBarImage;
        private Unit unit;
        private UnitHealthSystem healthSystem;

        private void Awake()
        {
            unit = GetComponentInParent<Unit>();
            healthSystem = GetComponentInParent<UnitHealthSystem>();
        }

        private void Start()
        {
            Unit.ON_ANY_ACTION_POINTS_CHANGED += Unit_OnAnyAction_Points_Changed;
            healthSystem.ON_UNIT_DAMAGED += HealthSystem_OnUnitDamaged;
            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void Unit_OnAnyAction_Points_Changed(object sender, EventArgs e)
        {
            UpdateActionPointsText();
        }

        private void HealthSystem_OnUnitDamaged(object sender, EventArgs e)
        {
            UpdateHealthBar();
        }

        private void UpdateActionPointsText()
        {
            actionPointsText.text = unit.GetActionPoints().ToString();
        }

        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }
    }
}