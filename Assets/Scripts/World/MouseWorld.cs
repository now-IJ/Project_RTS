using System;
using UnityEngine;

namespace RS
{
    public class MouseWorld : MonoBehaviour
    {

        private static MouseWorld instance;

        [SerializeField] private LayerMask mouseHitLayer;

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

        public static Vector3 GetMouseHitPosition()
        {
            RaycastHit hit;
            Ray cameraHitRay = Camera.main.ScreenPointToRay(InputManager.instance.GetMouseScreenPosition());
            Physics.Raycast(cameraHitRay, out hit, float.MaxValue, instance.mouseHitLayer);
            return hit.point;
        }
    }
}