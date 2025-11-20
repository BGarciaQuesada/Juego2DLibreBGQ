using UnityEngine;

public class MushroomAI : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 1f;
    public float waitTime = 2f;
    public float walkTime = 3f;

    [Header("Ataque")]
    public int attackDamage = 5;

    [Header("Estado")]
    public bool isDead = false;
    public bool isTakingDamage = false;

    private Animator anim;
    private Rigidbody2D rb;

    private float stateTimer;
    private bool walkingRight = true;

    // Estados internos
    private enum State { Idle, Walking }
    private State currentState = State.Idle;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateTimer = waitTime;

        // Inicia en idle
        anim.Play("Idle");
    }


    void Update()
    {
        if (isDead || isTakingDamage)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case State.Idle:
                anim.Play("Idle");

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                if (stateTimer <= 0)
                {
                    ChangeState(State.Walking);
                }
                break;

            case State.Walking:
                anim.Play("Run");

                float direction = walkingRight ? 1f : -1f;
                rb.linearVelocity = new Vector2(direction * walkSpeed, rb.linearVelocity.y);

                // Girar sprite
                transform.localScale = new Vector3(walkingRight ? -1f : 1f, 1f, 1f);

                if (stateTimer <= 0)
                {
                    walkingRight = !walkingRight;
                    ChangeState(State.Idle);
                }
                break;
        }
    }


    private void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.Idle)
            stateTimer = waitTime;
        else
            stateTimer = walkTime;
    }



    // --- ATAQUE ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isTakingDamage) return;

        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Attack");

            PlayerDamageReceiver player =
                other.GetComponent<PlayerDamageReceiver>();

            if (player != null)
                player.TakeDamage(5); // da�o fijo de la seta
        }
    }

    // --- DA�O RECIBIDO ---
    public void TakeDamage(int dmg)
    {
        if (isDead || isTakingDamage) return;

        isTakingDamage = true;
        anim.SetTrigger("Hurt");

        // Suponemos que tienes un MushroomHealth que maneja la vida
        GetComponent<MushroomHealth>()?.TakeDamage(dmg);
    }

    public void Die()
    {
        isDead = true;
        anim.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
