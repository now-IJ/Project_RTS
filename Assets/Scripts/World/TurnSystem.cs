using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem instance { get; private set; }
    
    public event EventHandler ON_TURN_CHANGED;

    private int turnNumber = 1;

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

    public void NextTurn()
    {
        turnNumber++;

        if (ON_TURN_CHANGED != null)
        {
            ON_TURN_CHANGED(this, EventArgs.Empty);
        }
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
