using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RS{
    public class ActionButtonUI : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;
        private Button button;
        private BaseAction baseAction;

        [SerializeField] private GameObject selectedVisual;

        private void Awake()
        {
            textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
        }

        public void SetBaseAction(BaseAction baseAction)
        {
            this.baseAction = baseAction;
            textMeshPro.text = baseAction.GetActionName().ToUpper();

            button.onClick.AddListener(() => { UnitActionSystem.instance.SetSelectedAction(baseAction); });
        }

        public void UpdateSelectedVisual()
        {
            BaseAction selectedBaseAction = UnitActionSystem.instance.GetSelecetedAction();
            selectedVisual.SetActive(selectedBaseAction == baseAction);
        }
    }
}
