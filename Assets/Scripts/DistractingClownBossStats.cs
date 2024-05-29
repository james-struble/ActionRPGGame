using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractingClownBossStats : MonoBehaviour, IDamageable
{
    public Event OnHurt;
    public Event OnDeath;
    private float health = 100f;
    private float maxHealth = 100f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetBossHealthNormalized()
    {
        return (health / maxHealth);
    }
}
