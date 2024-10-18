using System;
using Cinemachine;
using UnityEngine;

namespace RS
{
    public class CameraContoller : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        private Vector3 targetFollowOffset;
        private CinemachineTransposer cinemachineTransposer;

        [Header("Camera Speeds")] [SerializeField]
        private float cameraMoveSpeed = 8f;

        [SerializeField] private float cameraRotationSpeed = 8f;

        [Header("Camera Zooms")] [SerializeField]
        private float minZoom = 2f;

        [SerializeField] private float maxZoom = 12f;
        [SerializeField] private float zoomSpeed = 12f;


        private void Start()
        {
            cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        }

        // Update is called once per frame
        void Update()
        {
            HandleCameraMovement();
            HandleCameraRotation();
            HandleCameraZoom();
        }

        private void HandleCameraMovement()
        {
            Vector2 inputMoveDirection = InputManager.instance.GetCameraMoveVector();

            Vector3 moveDireciton = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
            transform.position += moveDireciton * cameraMoveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation()
        {
            Vector3 inputRotateVector = Vector3.zero;

            inputRotateVector.y = InputManager.instance.GetCameraRotateAmount();
          
            transform.eulerAngles += inputRotateVector * cameraRotationSpeed * Time.deltaTime;
        }

        private void HandleCameraZoom()
        {
            targetFollowOffset.y += InputManager.instance.GetCameraZoomAmount();

            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minZoom, maxZoom);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset,
                targetFollowOffset, Time.deltaTime * zoomSpeed);

        }
    }
}