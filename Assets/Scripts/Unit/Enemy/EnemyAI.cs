using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RS
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForEnemyTurn,
            TakingTurn,
            Busy,
        }

        private State state;
        private float timer;

        private void Awake()
        {
            state = State.WaitingForEnemyTurn;
        }

        private void Start()
        {
            TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
        }

        private void Update()
        {
            if (TurnSystem.instance.IsPlayerTurn())
            {
                return;
            }

            
            switch (state)
            {
                case State.WaitingForEnemyTurn:
                    break;
                case State.TakingTurn:
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            state = State.Busy;
                        }
                        else
                        {
                            // No more enemies have actions they can take, end enemy turn
                            TurnSystem.instance.NextTurn();
                        }
                    }

                    break;
                case State.Busy:
                    break;
            }
        }

        private void SetStateTakingTurn()
        {
            timer = 0.5f;
            state = State.TakingTurn;
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if (!TurnSystem.instance.IsPlayerTurn())
            {
                state = State.TakingTurn;
                timer = 2f;
            }
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
        {
            foreach (Unit enemyUnit in UnitManager.instance.GetEnemies())
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryTakeEnemyAIAction(Unit enemyUnit, Action OnEnemyAIActionComplete)
        {
            EnemyAIAction bestEnemyAIAction = null;
            BaseAction bestBaseAction = null;

            foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
            {
                if (!enemyUnit.HasActionPointToTakeAction(baseAction))
                {
                    // Enemy cannot afford this action
                    continue;
                }

                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAction();
                    if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = testEnemyAIAction;
                        bestBaseAction = baseAction;
                    }
                }

            }

            if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointToTakeAction(bestBaseAction))
            {
                bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, OnEnemyAIActionComplete);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}