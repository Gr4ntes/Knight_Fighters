using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public Ally_Movement ally_Movement;

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
            ally_Movement.ChangeState(AllyState.Dead);
            print("dead");
        }
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
