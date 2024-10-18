using System;
using UnityEngine;

namespace RS
{
    public class InteractSphere : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material materialOn;
        [SerializeField] private Material materialOff;

        private GridPosition gridPosition;
        
        private Action OnInteractComplete;
        private float timer;
        private bool isActive;

        private MeshRenderer meshRenderer;
        private bool turnedOn;
        
        private void Start()
        {
            gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
            LevelGrid.instance.SetInteractableAtGridPosition(gridPosition, this);
            Pathfinding.instance.SetIsWalkableGridPosition(gridPosition,false);
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            turnedOn = false;
            TurnOff();
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
        
        private void TurnOn()
        {
            turnedOn = true;
            meshRenderer.material = materialOn;
        }

        private void TurnOff()
        {
            turnedOn = false;
            meshRenderer.material = materialOff;
        }

        public void Interact(Action OnInteractComplete)
        {
            this.OnInteractComplete = OnInteractComplete;
            isActive = true;
            timer = 0.5f;
            if (turnedOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }
}