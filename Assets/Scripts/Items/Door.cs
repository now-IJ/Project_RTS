using System;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace RS
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool isOpen;

        private Action OnInteractComplete;
        private GridPosition gridPosition;
        private Animator animator;
        private float timer;
        private bool isActive;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
            LevelGrid.instance.SetInteractableAtGridPosition(gridPosition, this);

            if (isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        private void Update()
        {
            if(!isActive)
                return;
            
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isActive = false;
                OnInteractComplete();
            }
        }

        public void Interact(Action OnInteractComplete)
        {
            this.OnInteractComplete = OnInteractComplete;
            isActive = true;
            timer = 0.5f;
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        private void OpenDoor()
        {
            isOpen = true;
            animator.SetBool("IsOpen", isOpen);
            Pathfinding.instance.SetIsWalkableGridPosition(gridPosition,true);
        }

        private void CloseDoor()
        {
            isOpen = false;
            animator.SetBool("IsOpen", isOpen);
            Pathfinding.instance.SetIsWalkableGridPosition(gridPosition,false);
        }
    }
}