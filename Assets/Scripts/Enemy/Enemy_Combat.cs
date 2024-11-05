using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;
    public LayerMask allyLayer;
    public Enemy_Movement enemy_Movement;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(transform, knockbackForce, stunTime);
            enemy_Movement.ChangeState(EnemyState.Cooldown);
        }

        Collider2D[] ally_hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, allyLayer);

        if (ally_hits.Length > 0)
        {
            print("Attacked ally");
            ally_hits[0].GetComponent<Ally_Health>().ChangeHealth(-damage);
            ally_hits[0].GetComponent<Ally_Knockback>().Knockback(transform, knockbackForce, stunTime);
            enemy_Movement.ChangeState(EnemyState.Cooldown);
        }
    }
}
