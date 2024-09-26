using System;
using System.ComponentModel;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private GridPosition gridPosition;

    [SerializeField] private int actionPointsMax = 2;

    public static event EventHandler ON_ANY_ACTION_POINTS_CHANGED;
    
    private int actionPoints;
    private BaseAction[] baseActionArray;
    private MoveAction moveAction;
    private SpinAction spinAction;

    private void Awake()
    {
        actionPoints = actionPointsMax;
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        actionPoints = actionPointsMax;
        
        if (ON_ANY_ACTION_POINTS_CHANGED != null)
        {
            ON_ANY_ACTION_POINTS_CHANGED(this, EventArgs.Empty);
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

    public GridPosition GetGridPosition()
    {
        return gridPosition;
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
    
}
