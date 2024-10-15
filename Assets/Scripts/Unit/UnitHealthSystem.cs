using System;
using UnityEngine;

namespace RS
{
    public class UnitHealthSystem : MonoBehaviour
    {
        public event EventHandler ON_UNIT_DEATH;
        [SerializeField] private int health = 100;

        public void TakeDamage(int damageAmount)
        {
            health = Mathf.Clamp(health - damageAmount, 0, 100);

            if (health <= 0)
            {
                UnitDeath();
            }
        }

        private void UnitDeath()
        {
            if (ON_UNIT_DEATH != null)
            {
                ON_UNIT_DEATH(this, EventArgs.Empty);
            }
        }
    }
}