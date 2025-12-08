using System.Collections;
using UnityEngine;

public class GoblinAI : EnemyBaseAI
{
    [Header("Movimiento")]
    public float waitTime = 2f;

    [Header("Salto")]
    public float jumpForce = 7f;
    public float horizontalForce = 3f;      // Fuerza HORIZONTAL hacia el otro punto
    public float jumpDuration = 1f;
    public Transform[] jumpPoints;

    [Header("Sonidos Singulares")]
    [SerializeField] AudioClip GoblinJump;

    // Punto en el que está de entre los dos
    private int targetPoint = 0;

    private bool facingRight = false;

    // Esta booleana controla que el enemigo no salte 217931 veces de una
    private bool justEnteredJump = false;

    private enum State { Idle, Jumping }
    private State currentState = State.Idle;
    private float stateTimer;

    protected override void Start()
    {
        // Obtener anim y rigidbody
        base.Start();

        stateTimer = waitTime;

        // Sobreescribir stats
        attackDamage = 30;
        GetComponent<EnemyHealth>().maxHP = 300;
    }

    protected override void AIBehaviour()
    {
        if (jumpPoints.Length < 2)
            return;

        stateTimer -= Time.deltaTime;

        // Mucho texto, lo saco a métodos individuales......
        switch (currentState)
        {
            case State.Idle:
                IdleBehaviour();
                break;

            case State.Jumping:
                JumpingBehaviour();
                break;
        }
    }

    private void IdleBehaviour()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (stateTimer <= 0)
        {
            anim.SetTrigger("Jump");
            Jump();
            ChangeState(State.Jumping);
        }
    }

    private void JumpingBehaviour()
    {
        // Mantener el estado Jumping durante jumpDuration
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

        AudioManager.Instance.PlaySFX(GoblinJump);
    }

    private void ChangeState(State newState)
    {
        currentState = newState;

        switch (newState)
        {
            case State.Idle:
                stateTimer = waitTime;
                break;

            case State.Jumping:
                stateTimer = jumpDuration;
                break;
        }
    }

}
