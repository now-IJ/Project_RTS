using System;
using Cinemachine;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;
    
    [Header("Camera Speeds")] [SerializeField]
    private float cameraMoveSpeed = 8f;

    [SerializeField] private float cameraRotationSpeed = 8f;

    [Header("Camera Zooms")] 
    [SerializeField] private float minZoom = 2f;
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
        Vector3 inputMoveDirection = Vector3.zero;
        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = 1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = 1f;
        }

        Vector3 moveDireciton = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveDireciton * cameraMoveSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        Vector3 inputRotateVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            inputRotateVector.y = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            inputRotateVector.y = -1;
        }

        transform.eulerAngles += inputRotateVector * cameraRotationSpeed * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {
        float zoomAmount = 1f;
        
        if(Input.mouseScrollDelta.y >0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if(Input.mouseScrollDelta.y <0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minZoom, maxZoom);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);

    }
}
