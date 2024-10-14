using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RS
{
    public class EnemyAI : MonoBehaviour
    {
        private float timer;

        private void Start()
        {
            TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
        }

        private void Update()
        {
            if (TurnSystem.instance.IsPLayerTurn())
            {
                return;
            }

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                TurnSystem.instance.NextTurn();
            }
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            timer = 2f;
        }
    }
}