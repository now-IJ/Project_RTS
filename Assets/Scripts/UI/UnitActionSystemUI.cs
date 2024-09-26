using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
   [SerializeField] private GameObject actionButtonPrefab;
   [SerializeField] private Transform actionButtonContainer;
   [SerializeField] private TextMeshProUGUI actionPointsText;

   private List<ActionButtonUI> actionButtonUIList;

   private void Awake()
   {
      actionButtonUIList = new List<ActionButtonUI>();
   }

   private void Start()
   {
      UnitActionSystem.instance.ON_SELECTED_UNIT_CHANGED += UnitActionSystem_OnSelectedUnitChanged;
      UnitActionSystem.instance.ON_SELECTED_ACTION_CHANGED += UnitActionSystem_OnSelectedActionChanged;
      UnitActionSystem.instance.ON_ACTION_STARTED += UnitActionSystem_OnActionStarted;
      TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
      Unit.ON_ANY_ACTION_POINTS_CHANGED += Unit_OnAnyActionPointsChanged;
      UpdateSelectedVisual();
   }

   private void CreateUnitActionButtons()
   {
      foreach (Transform buttonGameObject in actionButtonContainer)
      {
         Destroy(buttonGameObject.gameObject);
      }
   
      actionButtonUIList.Clear();
      
      Unit selectedUnit = UnitActionSystem.instance.GetSelectedUnit();

      foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
      {
         GameObject actionButton = Instantiate(actionButtonPrefab, actionButtonContainer.transform);
         ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
         actionButtonUI.SetBaseAction(baseAction);
         actionButtonUIList.Add(actionButtonUI);
      }
   }

   private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
   {
      CreateUnitActionButtons();
      UpdateSelectedVisual();
      UpdateActionPoints();
   }

   private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
   {
      UpdateSelectedVisual();
   }

   private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
   {
      UpdateActionPoints();
   }

   private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
   {
      UpdateActionPoints();
   }

   private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
   {
      UpdateActionPoints();
   }
   
   private void UpdateSelectedVisual()
   {
      foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
      {
         actionButtonUI.UpdateSelectedVisual();
      }
   }

   private void UpdateActionPoints()
   {
      Unit selectedUnit = UnitActionSystem.instance.GetSelectedUnit();
      
      actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
   }
}
