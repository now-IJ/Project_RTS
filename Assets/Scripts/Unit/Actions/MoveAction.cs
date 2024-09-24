using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MoveAction : BaseAction
{
    private Animator animator;
    
    private Vector3 targetPosition;

    [Header("Movement Variables")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float reachTargetAcceptance = 0.1f;
    [SerializeField] private int maxMoveDistance = 4;
    
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (isActive)
        {
            HandleMovement();
        }
    }

    public void HandleMovement()
    {
        
        float distanceToTargetPosition = Vector3.Distance(transform.position, targetPosition);
        Vector3 moveDirection = targetPosition - transform.position;

        if (distanceToTargetPosition >= reachTargetAcceptance)
        {
            moveDirection.Normalize();
            transform.position += moveDirection * movementSpeed * Time.deltaTime;
            animator.SetBool("IsWalking", true);
        }
        else
        {
            OnActionComplete();
            isActive = false;
            if (!animator.GetBool("IsWalking")) 
                return;
            animator.SetBool("IsWalking", false);
        }
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }
    public void MoveToPosition(GridPosition gridPosition, Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        this.targetPosition = LevelGrid.instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public bool IsValidfActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validActionGridPositions = GetValidActionGridPositionList();
        return validActionGridPositions.Contains(gridPosition);
    }
    
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();
        
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if (LevelGrid.instance.HasUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }
    
}
