using System;
using System.ComponentModel;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Animator animator;
    
    private Vector3 targetPosition;
    private GridPosition gridPosition;
    
    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float reachTargetAcceptance = 0.01f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        LevelGrid.instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        HandleMovement();
    }

    public void MoveToPosition(Vector3 targetPosition)
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
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            if (!animator.GetBool("IsWalking")) 
                return;
            animator.SetBool("IsWalking", false);
        }
        
        GridPosition newGridPosition = LevelGrid.instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
        
    }
}
