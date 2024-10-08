using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep_Movement : MonoBehaviour
{
    public float speed;
    private int facingDirection = -1;
    private SheepState sheepState;
    private bool deadAnimFinished = false;
    
    public int healthRestore = 5;
    public float playerDetectRange = 3;
    public float playerCollideRange = .5f;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(SheepState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();

        if (sheepState == SheepState.Running)
        {
            Run();
        }

    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            player = hits[0].transform;
            if (sheepState == SheepState.Dead)
            {
                if (Vector2.Distance(transform.position, player.position) < playerCollideRange && deadAnimFinished)
                {
                    player.GetComponent<PlayerHealth>().ChangeHealth(healthRestore);
                    Destroy(gameObject);
                }
            }
            else
            {
                ChangeState(SheepState.Running);
            }
        }
        else if (sheepState != SheepState.Dead)
        {
            rb.velocity = Vector2.zero;
            ChangeState(SheepState.Idle);
        }
    }

    void Run()
    {
        if (player.position.x > transform.position.x && facingDirection == -1 ||
                player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = -direction * speed;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void DeadAnimationFinished()
    {
        deadAnimFinished = true;
    }

    public void ChangeState(SheepState newState)
    {
        // Exit the current animation
        if (sheepState == SheepState.Idle)
            anim.SetBool("isIdle", false);
        else if (sheepState == SheepState.Running)
            anim.SetBool("isRunning", false);
        else if (sheepState == SheepState.Dead)
            anim.SetBool("isDead", false);

        sheepState = newState;

        // Set the new animation
        if (sheepState == SheepState.Idle)
            anim.SetBool("isIdle", true);
        else if (sheepState == SheepState.Running)
            anim.SetBool("isRunning", true);
        else if (sheepState == SheepState.Dead)
            anim.SetBool("isDead", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerCollideRange);
    }

}

public enum SheepState
{
    Idle,
    Running,
    Dead,
}
