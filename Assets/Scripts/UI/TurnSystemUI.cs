using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace RS
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private Button nextTurnButton;
        [SerializeField] private GameObject enemyTurnBanner;

        private void Start()
        {
            nextTurnButton.onClick.AddListener(() => { TurnSystem.instance.NextTurn(); });

            TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;

            UpdateTurnText();
            UpdateEnemyTurnBanner();
            UpdateNextTurnButtonVisibility();
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateTurnText();
            UpdateEnemyTurnBanner();
            UpdateNextTurnButtonVisibility();
        }

        private void UpdateTurnText()
        {
            turnText.text = "Turn " + TurnSystem.instance.GetTurnNumber();
        }

        private void UpdateEnemyTurnBanner()
        {
            enemyTurnBanner.SetActive(!TurnSystem.instance.IsPLayerTurn());
        }

        private void UpdateNextTurnButtonVisibility()
        {
            nextTurnButton.gameObject.SetActive(TurnSystem.instance.IsPLayerTurn());
        }
    }
}
    