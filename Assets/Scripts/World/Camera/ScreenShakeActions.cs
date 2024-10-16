using System;
using UnityEngine;

namespace RS
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.ON_ANY_SHOOT += ShootAction_OnAnyShoot;
            GrenadeProjectile.ON_ANY_GRENADE_EXPLODED += GrenadeAction_OnAnyGrenadeExploded;
        }

        private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.instance.Shake(5f);
        }

        private void GrenadeAction_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.instance.Shake();            
        }
    }
}