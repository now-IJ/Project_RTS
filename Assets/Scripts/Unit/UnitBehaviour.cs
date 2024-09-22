using System;
using System.ComponentModel;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    private Animator animator;
    private Vector3 targetPosition;
    
    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float reachTargetAcceptance = 0.01f;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

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
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MoveToPosition(MouseWorld.GetMouseHitPosition());
        }
        
        float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTargetPosition >= reachTargetAcceptance)
        {
            Vector3 moveDirection = targetPosition - transform.position;
            moveDirection.Normalize();
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            if (!animator.GetBool("IsWalking")) 
                return;
            animator.SetBool("IsWalking", false);
        }
        
        
    }
}
