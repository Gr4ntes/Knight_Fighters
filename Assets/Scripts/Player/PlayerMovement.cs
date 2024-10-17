using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public int facingDirection = 1;
    public CinemachineVirtualCamera vCam;

    public Rigidbody2D rb;
    public Animator anim;

    private bool isKnockedBack;
    private bool isMovementDisabled;

    public Player_Combat player_Combat;

    private void Start()
    {
        CutsceneHandler.cutSceneStarted += DisableMovement;
        CutsceneHandler.cutSceneEnded += EnableMovement;
        CutsceneHandler.cutSceneEnded += setCameraFollow;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if (anim.GetBool("isAttacking"))
        {
            rb.velocity = Vector2.zero;
        }
        else if (!isKnockedBack && !isMovementDisabled)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
            {
                Flip();
            }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            rb.velocity = new Vector2 (horizontal, vertical) * speed;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.velocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void DisableMovement()
    {
        print("Movement disabled");
        isMovementDisabled = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("isAttacking", false);
        anim.SetFloat("horizontal", 0);
        anim.SetFloat("vertical", 0);
    }

    public void EnableMovement()
    {
        print("Movement enabled");
        isMovementDisabled = false;
    }

    public void setCameraFollow()
    {
        vCam.Follow = gameObject.transform;
    }
}