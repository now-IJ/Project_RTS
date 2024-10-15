using System;
using UnityEngine;

namespace RS
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject actionCamera;
        
        [Header("Shooting Action")]
        [SerializeField] private float shoulderOffsetAmount = 0.5f;

        private void Start()
        {
            BaseAction.ON_ANY_ACTION_START += Action_OnAnyActionStart;
            BaseAction.ON_ANY_ACTION_COMPLETED += Action_OnAnyActionCompleted;
        }

        private void Action_OnAnyActionStart(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    SetCameraToShootingPosition(shootAction);
                    ShowActionCamera();
                    break;
                default:
                    break;
            }
        }
        
        
        private void Action_OnAnyActionCompleted(object sender, EventArgs e)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    HideActionCamera();
                    break;
                default:
                    break;
            }
        }

        private void SetCameraToShootingPosition(ShootAction shootAction)
        {
            Unit shooterUnit = shootAction.GetUnit();
            Unit targetUnit = shootAction.GetTargetUnit();

            Vector3 cameraCharacterHeight = Vector3.up * 1.5f;

            Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

            Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

            Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);

            actionCamera.transform.position = actionCameraPosition;
            actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

        }
        
        private void ShowActionCamera()
        {
            actionCamera.gameObject.SetActive(true);
        }

        private void HideActionCamera()
        {
            actionCamera.gameObject.SetActive(false);
        }
    }
}