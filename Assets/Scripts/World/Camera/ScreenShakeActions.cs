using System;
using UnityEngine;

namespace RS
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.ON_ANY_SHOOT += ShootAction_OnAnyShoot;
        }

        private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.instance.Shake();
        }
    }
}