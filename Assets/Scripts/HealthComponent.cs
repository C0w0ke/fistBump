using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar?.SetFullHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar?.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            GetComponent<BaseCharacter>()?.Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthBar?.SetHealth(currentHealth);
    }
}

