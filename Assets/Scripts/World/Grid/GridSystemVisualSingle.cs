using System;
using UnityEngine;

namespace RS
{
    public class GridSystemVisualSingle : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public void Show(Material material)
        {
            meshRenderer.enabled = true;
            meshRenderer.material = material;
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }
    }
}