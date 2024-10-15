using System;
using UnityEngine;

namespace RS
{
    public class UnitHealthSystem : MonoBehaviour
    {
        public event EventHandler ON_UNIT_DEATH;
        public event EventHandler ON_UNIT_DAMAGED;
        [SerializeField] private int health = 100;
        private int maxHealth = 100;

        private void Awake()
        {
            maxHealth = health;
        }

        public void TakeDamage(int damageAmount)
        {
            health = Mathf.Clamp(health - damageAmount, 0, 100);

            if (ON_UNIT_DAMAGED != null)
            {
                ON_UNIT_DAMAGED(this, EventArgs.Empty);
            }
            
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

        public float GetHealthNormalized()
        {
            return (float)health / maxHealth;
        }
    }
}