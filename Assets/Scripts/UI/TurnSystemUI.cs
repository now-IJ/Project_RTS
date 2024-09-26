using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button nextTurnButton;

    private void Start()
    {
        nextTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.instance.NextTurn();
        });

        TurnSystem.instance.ON_TURN_CHANGED += TurnSystem_OnTurnChanged;
        
        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
    
    private void UpdateTurnText()
    {
        turnText.text = "Turn " + TurnSystem.instance.GetTurnNumber();
    }
}
    