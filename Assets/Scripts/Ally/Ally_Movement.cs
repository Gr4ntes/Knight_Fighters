using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ally_Movement : MonoBehaviour
{
    public float speed;
    private int facingDirection = 1;
    private AllyState allyState;
    private float attackCooldownTimer;

    public int damage = 1;
    public float DetectRange = 3;
    public float enemyDetectRange = 4;
    public float attackRange = 2;
    public float attackCooldown = 2;
    
    public Transform detectionPoint;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Transform followTarget;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(AllyState.Idle);
        attackCooldown = Random.Range(1f, 2f);
        print(attackCooldown);
    }

    private void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            ChangeState(AllyState.Idle);
        }

        if (allyState != AllyState.Knockback && allyState != AllyState.Dead && allyState != AllyState.Cooldown)
        {
            CheckForEnemy();
            if (allyState != AllyState.Chasing && allyState != AllyState.Attacking)
            {
                CheckForPlayer();
            } 

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (allyState == AllyState.Attacking || attackCooldownTimer > 0 || followTarget == null)
            {
                rb.velocity = Vector2.zero;
            }
            else if (allyState == AllyState.Following || allyState == AllyState.Chasing)
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
        if (followTarget.position.x > transform.position.x && facingDirection == -1 ||
                followTarget.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (followTarget.position - transform.position).normalized;
        rb.velocity = direction * speed;
        
    }

    public void ChangeState(AllyState newState)
    {
        // Exit the current animation
        if (allyState == AllyState.Idle)
            anim.SetBool("isIdle", false);
        else if (allyState == AllyState.Following || allyState == AllyState.Chasing)
            anim.SetBool("isFollowing", false);
        else if (allyState == AllyState.Attacking)
            anim.SetBool("isAttacking", false);
        else if (allyState == AllyState.Dead)
            anim.SetBool("isDead", false);

        allyState = newState;

        // Set the new animation
        if (allyState == AllyState.Idle)
            anim.SetBool("isIdle", true);
        else if (allyState == AllyState.Following || allyState == AllyState.Chasing)
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
            followTarget = hits[0].transform;

            rb.velocity = Vector2.zero;
            ChangeState(AllyState.Idle);
        }
        else if (hits.Length == 0 &&  followTarget != null)
        {
            ChangeState(AllyState.Following);
        }
    }

    private void CheckForEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, enemyDetectRange, enemyLayer);

        if (hits.Length > 0)
        {
            followTarget = hits[0].transform;
            if (Vector2.Distance(transform.position, followTarget.position) < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                attackCooldown = Random.Range(1f, 2f);
                print(attackCooldown);
                ChangeState(AllyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, followTarget.position) > attackRange && allyState != AllyState.Attacking)
            {
                ChangeState(AllyState.Chasing);
            }
        }
    }

    
}

public enum AllyState
{
    Idle,
    Following,
    Chasing,
    Attacking,
    Knockback,
    Cooldown,
    Dead
}
