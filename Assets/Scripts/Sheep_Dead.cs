using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep_Dead : MonoBehaviour
{
    private Rigidbody2D rb;
    private Sheep_Movement sheep_Movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sheep_Movement = GetComponent<Sheep_Movement>();
    }

    public void Dead()
    {
        sheep_Movement.ChangeState(SheepState.Dead);
        rb.velocity = Vector2.zero;
    }
}
