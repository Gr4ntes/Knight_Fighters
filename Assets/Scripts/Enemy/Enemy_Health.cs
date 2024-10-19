using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public Enemy_Movement enemy_Movement;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        else if (currentHealth <= 0)
        {
            enemy_Movement.ChangeState(EnemyState.Dead);
            print("dead");
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
