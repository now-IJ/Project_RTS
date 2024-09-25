using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
   [SerializeField] private GameObject actionButtonPrefab;
   [SerializeField] private Transform actionButtonContainer;

   private List<ActionButtonUI> actionButtonUIList;

   private void Awake()
   {
      actionButtonUIList = new List<ActionButtonUI>();
   }

   private void Start()
   {
      UnitActionSystem.instance.ON_SELECTED_UNIT_CHANGED += UnitActionSystem_OnSelectedUnitChanged;
      UnitActionSystem.instance.ON_SELECTED_ACTION_CHANGED += UnitActionSystem_OnSelectedActionChanged;
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
   }

   private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
   {
      UpdateSelectedVisual();
   }

   private void UpdateSelectedVisual()
   {
      foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
      {
         actionButtonUI.UpdateSelectedVisual();
      }
   }
}
