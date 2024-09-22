using System;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    private Vector3 targetPosition;
    private float movementSpeed = 4f;
    private float reachTargetAcceptance = 0.01f;
    
    private void Update()
    {
        HandleMovement();
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
       
    }

    private void HandleMovement()
    {
        float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTargetPosition >= reachTargetAcceptance)
        {
            Vector3 moveDirection = targetPosition - transform.position;
            moveDirection.Normalize();
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MoveToPosition(MouseWorld.GetMouseHitPosition());
        }
    }
}
