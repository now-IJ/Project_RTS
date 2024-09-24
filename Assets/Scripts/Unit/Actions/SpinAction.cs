using System;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpin;
    
    private void Update()
    {
        if (isActive)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            totalSpin += spinAddAmount;
            if (totalSpin >= 360)
            {
                isActive = false;
                OnActionComplete();
            }
        }
    }

    public void Spin(Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        isActive = true;
        totalSpin = 0;
    }
}
