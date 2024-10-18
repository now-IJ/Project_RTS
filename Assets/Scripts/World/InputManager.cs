using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RS
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        private PlayerInputs playerInputs;
        
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

            playerInputs = new PlayerInputs();
        }

        private void Start()
        {
            playerInputs.Enable();
        }

        public Vector2 GetMouseScreenPosition()
        {
            return Mouse.current.position.ReadValue();
        }

        public bool IsMouseButtonDown()
        {
            return playerInputs.Player.Interact.WasPressedThisFrame();
        }

        public Vector2 GetCameraMoveVector()
        {
            return playerInputs.Camera.Movement.ReadValue<Vector2>();
        }

        public float GetCameraRotateAmount()
        {
            return playerInputs.Camera.Rotate.ReadValue<float>();
        }
        
        public float GetCameraZoomAmount()
        {
            return playerInputs.Camera.Zoom.ReadValue<float>();
        }
    }
}