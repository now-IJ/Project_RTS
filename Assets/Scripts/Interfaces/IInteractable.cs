using System;

namespace RS
{
    public interface IInteractable
    {
        void Interact(Action OnInteractComplete);
    }
}   