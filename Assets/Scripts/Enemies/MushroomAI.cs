using System.Collections;
using UnityEngine;

public class MushroomAI : EnemyBaseAI
{
    [Header("Movimiento")]
    public float moveSpeed = 1f;
    public float waitTime = 2f;
    public float walkTime = 3f;
    
    private bool facingRight = false;

    private enum State { Idle, Walking }
    private State currentState = State.Idle;
    private float stateTimer;


    // Protected para que solo se vean en la misma línea de heredación
    protected override void Start()
    {
        // Obtener anim y rigidbody
        base.Start();

        stateTimer = waitTime;

        // Sobreescribir stats
        attackDamage = 10;
        GetComponent<EnemyHealth>().maxHP = 100;

        anim.SetBool("Walking", false);
    }


    protected override void AIBehaviour()
    {
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Idle:
                anim.SetBool("Walking", false);
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                if (stateTimer <= 0)
                    ChangeState(State.Walking);
                break;

            case State.Walking:
                anim.SetBool("Walking", true);
                float dir = facingRight ? 1f : -1f;

                rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
                transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

                if (stateTimer <= 0)
                {
                    facingRight = !facingRight;
                    ChangeState(State.Idle);
                }
                break;
        }
    }


    private void ChangeState(State newState)
    {
        currentState = newState;
        stateTimer = newState == State.Idle ? waitTime : walkTime;
    }
}
