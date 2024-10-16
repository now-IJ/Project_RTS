using TMPro;
using UnityEngine;

namespace RS
{
    public class PathfindingGridDebug : GridDebugObject
    {
        [SerializeField] private TextMeshPro gCostText;
        [SerializeField] private TextMeshPro hCostText;
        [SerializeField] private TextMeshPro fCostText;

        private PathNode pathNode;

        protected override void Awake()
        {
        }

        public override void SetGridObject(object gridObject)
        {
            pathNode = (PathNode)gridObject;
            
            base.SetGridObject(gridObject);
        }

        protected override void Update()
        {
            base.Update();
            gCostText.text = pathNode.GetGCost().ToString();
            hCostText.text = pathNode.GetHCost().ToString();
            fCostText.text = pathNode.GetFCost().ToString();
        }
    }
}