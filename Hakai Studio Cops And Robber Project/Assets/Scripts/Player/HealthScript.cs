using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
