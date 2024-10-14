using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RS
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem instance { get; private set; }

        public event EventHandler ON_SELECTED_UNIT_CHANGED;
        public event EventHandler ON_SELECTED_ACTION_CHANGED;
        public event EventHandler ON_ACTION_STARTED;
        public event EventHandler ON_ACTION_RUNNING;
        public event EventHandler ON_ACTION_CLEAR;


        private Unit selectedUnit;
        private BaseAction selectedAction;
        private bool isActionRunning;

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

        private void Start()
        {
            SetSelectedUnit(selectedUnit);
        }

        private void Update()
        {
            if (isActionRunning)
            {
                return;
            }

            if (!TurnSystem.instance.IsPLayerTurn())
            {
                return;
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (TryHandleUnitSelection())
            {
                return;
            }

            HandleSelectedAction();

        }

        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.instance.GetGridPosition(MouseWorld.GetMouseHitPosition());

                if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    if (selectedUnit.TrySpendActionPointToTakeAction(selectedAction))
                    {
                        SetActionRunning();
                        selectedAction.TakeAction(mouseGridPosition, ClearActionRunning);
                        if (ON_ACTION_STARTED != null)
                        {
                            ON_ACTION_STARTED(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        private void SetActionRunning()
        {
            isActionRunning = true;
            if (ON_ACTION_RUNNING != null)
            {
                ON_ACTION_RUNNING(this, EventArgs.Empty);
            }
        }

        private void ClearActionRunning()
        {
            isActionRunning = false;
            if (ON_ACTION_CLEAR != null)
            {
                ON_ACTION_CLEAR(this, EventArgs.Empty);
            }
        }

        private bool TryHandleUnitSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray cameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(cameraHitRay, out hit, float.MaxValue, unitsLayer))
                {
                    if (hit.collider.gameObject.TryGetComponent<Unit>(out Unit clickedOnUnit))
                    {
                        if (clickedOnUnit == selectedUnit)
                        {
                            // Unit is already selected
                            return false;
                        }

                        if (clickedOnUnit.IsEnemy())
                        {
                            // Unit is enemy
                            return false;
                        }
                        SetSelectedUnit(clickedOnUnit);
                        return true;
                    }
                }
            }

            return false;
        }


        private void SetSelectedUnit(Unit clickedOnUnit)
        {
            selectedUnit = clickedOnUnit;
            SetSelectedAction(selectedUnit.GetMoveAction());

            if (ON_SELECTED_UNIT_CHANGED != null)
            {
                ON_SELECTED_UNIT_CHANGED(this, EventArgs.Empty);
            }
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            selectedAction = baseAction;

            if (ON_SELECTED_ACTION_CHANGED != null)
            {
                ON_SELECTED_ACTION_CHANGED(this, EventArgs.Empty);
            }
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public BaseAction GetSelecetedAction()
        {
            return selectedAction;
        }
    }
}