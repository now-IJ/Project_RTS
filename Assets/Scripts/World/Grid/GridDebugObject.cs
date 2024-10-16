using System;
using TMPro;
using UnityEngine;

namespace RS
{
    public class GridDebugObject : MonoBehaviour
    {
        private GridObject gridObject;

        private TextMeshPro debugGridPositionText;

        private void Awake()
        {
            debugGridPositionText = GetComponentInChildren<TextMeshPro>();
        }

        public void SetGridObject(GridObject gridObject)
        {
            this.gridObject = gridObject;


        }

        private void Update()
        {
            //debugGridPositionText.SetText(gridObject.ToString());
        }
    }
}