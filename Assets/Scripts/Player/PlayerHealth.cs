using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthText;
    public Animator healthTextAnim;
    public Animator playerAnim;

    private void Start()
    {
        healthText.text = "HP:" + currentHealth + "/" + maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        if (amount + currentHealth > maxHealth)
        {
            amount = maxHealth - currentHealth;
        }
        
        currentHealth += amount;
        healthTextAnim.Play("TextUpdate");
        healthText.text = "HP:" + currentHealth + "/" + maxHealth;

        if (currentHealth <= 0)
        {
            PlayerMovement.playerDead?.Invoke();
            playerAnim.SetBool("isDead", true);
        }

    }

    public void Dead()
    {
        print("died");
        gameObject.SetActive(false);
    }
}
