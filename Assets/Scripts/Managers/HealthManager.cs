using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Managers
{
    public class HealthManager : MonoBehaviour
    {
        [Serializable] public class DamageEvent : UnityEvent<HealthManager, float> {}
        [Serializable] public class DeathEvent : UnityEvent<HealthManager> {}
        [Serializable] public class HealEvent : UnityEvent<HealthManager, float> {}
    
        public float Health => health;
        public bool IsDead => health <= 0 || hitsLeft <= 0;
        public bool AbsorbBulletsWhenDead => absorbBulletsWhenDead;
    
        [SerializeField] private float maxHealth = 100;
        [SerializeField] private bool absorbBulletsWhenDead;
        [SerializeField] private bool disableOnDeath = true;
        [SerializeField] private float health = 100;
        [SerializeField] private float hitsLeft = 1;
        [SerializeField] private bool hitBasedHealth;
        public DamageEvent OnDamageTaken;
        public DeathEvent OnDeath;
        public HealEvent OnHealed;

        void TakeDamage(float damage)
        {
            if (!hitBasedHealth)
            {
                health -= Mathf.Abs(damage);
            }
            else
            {
                hitsLeft -= 1;
            }
        
            OnDamageTaken.Invoke(this, damage);
            if (health <= 0 || hitsLeft <= 0)
            {
                OnDeath.Invoke(this);
                if (disableOnDeath)
                {
                    enabled = false;
                }
            }
        }

        void Heal(float damage)
        {
            health = Mathf.Min(health + Mathf.Abs(damage), maxHealth);
            OnHealed.Invoke(this, damage);
        }

        public void ApplyDamage(float damage)
        {
            TakeDamage(damage);
        }
    }
}
