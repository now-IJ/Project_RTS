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
            SwordAction.ON_ANY_SWORD_HIT += SwordAction_OnAnySwordHit;
        }
        
        private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.instance.Shake(5f);
        }

        private void GrenadeAction_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.instance.Shake();            
        }
        
        private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
        {
            ScreenShake.instance.Shake(2f);      
        }
    }
}