using System;
using TMPro;
using UnityEngine;

namespace RS
{
    public class GridDebugObject : MonoBehaviour
    {
        private object gridObject;

        [SerializeField] private TextMeshPro debugGridPositionText;

        protected virtual void Awake()
        {
            debugGridPositionText = GetComponentInChildren<TextMeshPro>();
        }

        public virtual void SetGridObject(object gridObject)
        {
            this.gridObject = gridObject;
            
        }

        protected virtual void Update()
        {
            //debugGridPositionText.SetText(gridObject.ToString());
        }
    }
}