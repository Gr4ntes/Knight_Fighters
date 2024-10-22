using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    public LayerMask enemyLayer;
    public Ally_Movement ally_Movement;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

        if (hits.Length > 0)
        {
            print("hit");
            hits[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);
            hits[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            ally_Movement.ChangeState(AllyState.Cooldown);
        }
    }
}
