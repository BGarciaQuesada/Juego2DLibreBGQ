using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 7.5f;

    private StatChange currentInteractable;

    private Animator anim;
    private Rigidbody2D rb;

    private Sensor_Player groundSensor;
    private bool grounded = false;
    private Vector2 moveInput;

    private bool inCombatMode = false;  // idle alternativo, tendré que crear un scene manager entre escenas para cambiar de idle pero bleh
    private bool canMove = true;        // se desactiva si está muerto
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("PlayerSensor").GetComponent<Sensor_Player>();
    }

    // --- INPUTS ---
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (!grounded || !canMove) return;

        anim.SetTrigger("Jump");
        grounded = false;
        anim.SetBool("Grounded", false);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // Evitar detección de suelo por un tiempo
        groundSensor.Disable(0.2f);
    }

    void OnAttack(InputValue value)
    {
        if (!canMove) return;

        // PlayerCombat ya se encarga del ataque
        // Literal solo existe para el Input System
    }

    // No se en qué momento borré esto, creo que fue al duplicar el código para hacer el del combate demonios
    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            currentInteractable?.Interact();
        }
    }

    public void SetInteractable(StatChange sc)
    {
        currentInteractable = sc;
    }


    // --- UPDATE ---
    // Aka. Animaciones y que esté tocando el suelo mientras no este muerto = no se pueda mover
    void Update()
    {
        if (!canMove) return;

        HandleGroundCheck();
        HandleMovementAnimations();
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Move();
    }

    // --- MANAGER DE MOVIMIENTOS ---
    // Aka. Velocidad y ver si está en el suelo
    private void Move()
    {
        float x = moveInput.x;

        // Aplicar movimiento
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Rotación del sprite
        if (x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void HandleGroundCheck()
    {
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            anim.SetBool("Grounded", true);
        }
        else if (grounded && !groundSensor.State())
        {
            grounded = false;
            anim.SetBool("Grounded", false);
        }
    }

    // --- ANIMACIONES ---
    private void HandleMovementAnimations()
    {
        float x = Mathf.Abs(moveInput.x);

        // Idle de combate
        // anim.SetBool("CombatMode", inCombatMode); //WIP

        // Caminar
        anim.SetBool("Walking", x > 0.1f && grounded);

        // Velocidad vertical
        anim.SetFloat("AirSpeedY", rb.linearVelocity.y);
    }

    // --- FUNCIONES DE OTROS SCRIPTS ---

    public void SetCombatMode(bool active)
    {
        inCombatMode = active;
        anim.SetBool("CombatMode", active);
    }

    public void PlayHurtAnimation()
    {
        anim.SetTrigger("Hurt");
    }

    public void Die()
    {
        canMove = false;
        anim.SetTrigger("Die");
    }
}
