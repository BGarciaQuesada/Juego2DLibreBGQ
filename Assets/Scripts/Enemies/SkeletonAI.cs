using System.Collections;
using UnityEngine;

public class SkeletonAI : EnemyBaseAI
{
    [Header("Movimiento")]
    public float moveSpeed = 1f;
    public float waitTime = 2f;
    public float walkTime = 3f;

    [Header("Salto")]
    public float jumpForce = 7f;
    public float horizontalForce = 3f;      // Fuerza HORIZONTAL hacia el otro punto
    public float jumpDuration = 1f;
    public Transform[] jumpPoints;

    // Punto en el que está de entre los dos
    private int targetPoint = 0;

    private bool facingRight = false;

    // Esta booleana controla que el enemigo no salte 217931 veces de una
    private bool justEnteredJump = false;

    private enum State { Idle, Walking, Jumping }
    private State currentState = State.Idle;
    private float stateTimer;

    protected override void Start()
    {
        // Obtener anim y rigidbody
        base.Start();

        stateTimer = waitTime;

        // Sobreescribir stats
        attackDamage = 50;
        GetComponent<EnemyHealth>().maxHP = 500;
    }

    protected override void AIBehaviour()
    {
        stateTimer -= Time.deltaTime;

        // Mucho texto, lo saco a métodos individuales......
        switch (currentState)
        {
            case State.Idle:
                IdleBehaviour();
                break;

            case State.Walking:
                WalkingBehaviour();
                break;

            case State.Jumping:
                JumpingBehaviour();
                break;
        }
    }

    private void IdleBehaviour()
    {
        if (stateTimer <= 0)
        {
            anim.SetBool("Walking", true);
            ChangeState(State.Walking);
        }
    }

    private void WalkingBehaviour()
    {
        // Andar hacia el siguiente punto horizontal
        Vector2 target = jumpPoints[targetPoint].position;
        float directionX = target.x - rb.position.x;
        facingRight = directionX > 0;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        // Mover horizontalmente usando MovePosition
        Vector2 movement = new Vector2(Mathf.Sign(directionX) * moveSpeed * Time.fixedDeltaTime, 0);
        rb.MovePosition(rb.position + movement);

        if (stateTimer <= 0)
        {
            anim.SetBool("Walking", false);
            ChangeState(State.Jumping);
        }
    }

    private void JumpingBehaviour()
    {
        if (justEnteredJump)
        {
            anim.SetTrigger("Jump");

            Jump();
            justEnteredJump = false;
        }

        if (stateTimer <= 0)
        {
            ChangeState(State.Idle);
        }
    }

    private void Jump()
    {
        int next = (targetPoint + 1) % jumpPoints.Length;

        Vector2 direction = (jumpPoints[next].position - transform.position).normalized;

        // Determinar hacia dónde mira
        facingRight = direction.x > 0;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        rb.linearVelocity = new Vector2(direction.x * horizontalForce, jumpForce);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // impulso vertical único

        targetPoint = next;
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                stateTimer = waitTime;
                break;
            case State.Walking:
                stateTimer = walkTime;
                break;
            case State.Jumping:
                stateTimer = jumpDuration;
                justEnteredJump = true;
                break;
        }
    }

}
