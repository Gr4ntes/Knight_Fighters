using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectRange = 3;
    public Transform detectionPoint;
    public LayerMask playerLayer;
    public LayerMask alliesLayer;

    private float attackCooldownTimer;
    private int facingDirection = -1;
    private EnemyState enemyState;

    private Rigidbody2D rb;
    private Transform attackTarget;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
        attackCooldown = Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else if (attackCooldown == 0 && enemyState != EnemyState.Dead)
        {
            ChangeState(EnemyState.Idle);
        }

        if (enemyState != EnemyState.Knockback && enemyState != EnemyState.Dead && enemyState != EnemyState.Cooldown)
        {
            CheckForPlayer();
        
        
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    void Chase()
    {
        if (attackTarget.position.x > transform.position.x && facingDirection == -1 ||
                attackTarget.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (attackTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits_player = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
        Collider2D[] hits_allies = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, alliesLayer);

        if (hits_player.Length > 0 || hits_allies.Length > 0)
        {
            if (hits_allies.Length > 0) 
                attackTarget = hits_allies[0].transform;
            else if (hits_player.Length > 0)
                attackTarget = hits_player[0].transform;

            if (Vector2.Distance(transform.position, attackTarget.position) < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                attackCooldown = Random.Range(1f, 2f);
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, attackTarget.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }  
    }

    public void ChangeState(EnemyState newState)
    {
        // Exit the current animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);
        else if (enemyState == EnemyState.Dead)
            anim.SetBool("isDead", false);

        enemyState = newState;

        // Set the new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
        else if (enemyState == EnemyState.Dead)
            anim.SetBool("isDead", true);
    }

    public EnemyState getState()
    {
        return enemyState;
    }
}


public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    Cooldown,
    Dead
}