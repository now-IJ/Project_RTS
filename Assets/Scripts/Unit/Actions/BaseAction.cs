using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action OnActionComplete;
    protected bool isActive;
    protected Unit unit;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
}
