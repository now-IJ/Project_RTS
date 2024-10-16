using System;
using System.ComponentModel;
using UnityEngine;

namespace RS{
    public class Unit : MonoBehaviour
    {

        private GridPosition gridPosition;

        [SerializeField] private int actionPointsMax = 2;

        public static event EventHandler ON_ANY_ACTION_POINTS_CHANGED;
        public static event EventHandler ON_ANY_UNIT_SPAWNED;
        public static event EventHandler ON_ANY_UNIT_DEAD;

        [SerializeField] private bool isEnemy;

        private int actionPoints;
        private UnitHealthSystem healthSystem;
        private BaseAction[] baseActionArray;
        private MoveAction moveAction;
        private SpinAction spinAction;
        private ShootAction shootAction;

        private void Awake()
        {
            actionPoints = actionPointsMax;
            healthSystem = GetComponent<UnitHealthSystem>();
            moveAction = GetComponent<MoveAction>();
            spinAction = GetComponent<SpinAction>();
            shootAction = GetComponent<ShootAction>();
            baseActionArray = GetComponents<BaseAction>();
        }

        private void Start()
        {
            TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
            healthSystem.ON_UNIT_DEATH += HealthSystem_OnUnitDeath;
            gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
            LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);

            if (ON_ANY_UNIT_SPAWNED != null)
            {
                ON_ANY_UNIT_SPAWNED(this, EventArgs.Empty);
            }
        }

        private void Update()
        {
            GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                GridPosition oldGridPosition = gridPosition;
                gridPosition = newGridPosition;
                LevelGrid.instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
            }
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if ((IsEnemy() && !TurnSystem.instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.instance.IsPlayerTurn()))
            {
                actionPoints = actionPointsMax;

                if (ON_ANY_ACTION_POINTS_CHANGED != null)
                {
                    ON_ANY_ACTION_POINTS_CHANGED(this, EventArgs.Empty);
                }
            }
        }

        public BaseAction[] GetBaseActionArray()
        {
            return baseActionArray;
        }

        public MoveAction GetMoveAction()
        {
            return moveAction;
        }

        public SpinAction GetSpinAction()
        {
            return spinAction;
        }

        public ShootAction GetShootAction()
        {
            return shootAction;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public bool TrySpendActionPointToTakeAction(BaseAction baseAction)
        {
            if (HasActionPointToTakeAction(baseAction))
            {
                SpendActionPoint(baseAction.GetActionPointsCost());
                return true;
            }

            return false;
        }

        public bool HasActionPointToTakeAction(BaseAction baseAction)
        {
            return actionPoints >= baseAction.GetActionPointsCost();
        }

        private void SpendActionPoint(int amount)
        {
            actionPoints -= amount;

            if (ON_ANY_ACTION_POINTS_CHANGED != null)
            {
                ON_ANY_ACTION_POINTS_CHANGED(this, EventArgs.Empty);
            }
        }

        public int GetActionPoints()
        {
            return actionPoints;
        }

        public bool IsEnemy()
        {
            return isEnemy;
        }

        public void Damage(int damageAmount)
        {
            healthSystem.TakeDamage(damageAmount);
        }
        
        private void HealthSystem_OnUnitDeath(object sender, EventArgs e)
        {
            if (ON_ANY_UNIT_DEAD != null)
            {
                ON_ANY_UNIT_DEAD(this, EventArgs.Empty);
            }
            LevelGrid.instance.ClearUnitAtGridPosition(gridPosition, this);
            Destroy(gameObject);
        }

        public float GetHealthNormalized()
        {
            return healthSystem.GetHealthNormalized();
        }
    }
}
