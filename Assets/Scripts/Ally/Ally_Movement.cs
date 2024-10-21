using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ally_Movement : MonoBehaviour
{
    public float speed;
    private int facingDirection = -1;
    private AllyState allyState;

    public int damage = 1;
    public float DetectRange = 3;
    public float AttackRange = 2;
    public Transform detectionPoint;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(AllyState.Disabled);
    }

    private void Update()
    {
        if (allyState != AllyState.Knockback && allyState != AllyState.Dead)
        {
            CheckForPlayer();

            if (allyState == AllyState.Following)
            {
                Follow();
            }
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void Follow()
    {
        if (player.position.x > transform.position.x && facingDirection == -1 ||
                player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void ChangeState(AllyState newState)
    {
        // Exit the current animation
        if (allyState == AllyState.Disabled)
            anim.SetBool("isDisabled", false);
        else if (allyState == AllyState.Idle)
            anim.SetBool("isIdle", false);
        else if (allyState == AllyState.Following)
            anim.SetBool("isFollowing", false);
        else if (allyState == AllyState.Attacking)
            anim.SetBool("isAttacking", false);
        else if (allyState == AllyState.Dead)
            anim.SetBool("isDead", false);

        allyState = newState;

        // Set the new animation
        if (allyState == AllyState.Disabled)
            anim.SetBool("isDisabled", true);
        else if (allyState == AllyState.Idle)
            anim.SetBool("isIdle", true);
        else if (allyState == AllyState.Following)
            anim.SetBool("isFollowing", true);
        else if (allyState == AllyState.Attacking)
            anim.SetBool("isAttacking", true);
        else if (allyState == AllyState.Dead)
            anim.SetBool("isDead", true);
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, DetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;

            rb.velocity = Vector2.zero;
            ChangeState(AllyState.Idle);
        }
        else if (hits.Length == 0 &&  player != null && allyState != AllyState.Disabled)
        {
            ChangeState(AllyState.Following);
        }
    }
}

public enum AllyState
{
    Disabled,
    Idle,
    Following,
    Attacking,
    Knockback,
    Dead
}
