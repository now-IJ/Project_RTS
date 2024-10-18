using System;
using UnityEngine;

namespace RS
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

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

        public Vector2 GetMouseScreenPosition()
        {
            return Input.mousePosition;
        }

        public bool IsMouseButtonDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public Vector2 GetCameraMoveVector()
        {
            Vector2 inputMoveDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDirection.y = 1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDirection.x = -1f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDirection.y = -1f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDirection.x = 1f;
            }

            return inputMoveDirection;
        }

        public float GetCameraRotateAmount()
        {
            float rotateAmount = 0.0f;
            if (Input.GetKey(KeyCode.Q))
            {
                rotateAmount = 1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotateAmount = -1f;
            }

            return rotateAmount;
        }
        
        public float GetCameraZoomAmount()
        {
            float zoomAmount = 0.0f;
            
            if (Input.mouseScrollDelta.y > 0)
            {
                zoomAmount = -1f;
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                zoomAmount = +1f;
            }

            return zoomAmount;
        }
    }
}