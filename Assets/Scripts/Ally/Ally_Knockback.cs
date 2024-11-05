using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Ally_Movement ally_Movement;
    public float knockbackTime = 0.15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ally_Movement = GetComponent<Ally_Movement>();
    }

    public void Knockback(Transform playerTransform, float knockbackForce, float stunTime)
    {
        if (ally_Movement.getState() != AllyState.Dead)
        {
            ally_Movement.ChangeState(AllyState.Knockback);
            StartCoroutine(StunTimer(knockbackTime, stunTime));
            Vector2 direction = (transform.position - playerTransform.position).normalized;
            rb.velocity = direction * knockbackForce;
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        ally_Movement.ChangeState(AllyState.Idle);
    }
}
