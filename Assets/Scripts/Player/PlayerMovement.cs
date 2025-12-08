using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 7.5f;

    [Header("Sonidos Singulares")]
    [SerializeField] AudioClip PlayerJump;
    [SerializeField] AudioClip PauseSound;

    // Necesito un AudioSource para usar la propiedad volume en los efectos en bucle...
    [Header("Audio Bucle Pasos")]
    [SerializeField] private AudioSource footsteps;

    // MonoBehavior porque puede ser StatChange o ChangeScene
    private MonoBehaviour currentInteractable;

    private Animator anim;
    private Rigidbody2D rb;

    private Sensor_Player groundSensor;
    private bool grounded = false;
    private Vector2 moveInput;

    private bool inCombatMode = false;  // idle alternativo, tendré que crear un scene manager entre escenas para cambiar de idle pero bleh
    private bool canMove = true;        // se desactiva si está muerto

    public static PlayerMovement Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

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

        AudioManager.Instance.PlaySFX(PlayerJump);

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
        if (value.isPressed && currentInteractable != null)
        {
            // Obtener metodo Interact si lo tiene y llamarlo
            var method = currentInteractable.GetType().GetMethod("Interact");
            method?.Invoke(currentInteractable, null);
        }
    }

    // El método tiene que ir aquí porque es el que tiene el input manager
    void OnPause()
    {
        PauseManager.Instance.TogglePause();
        AudioManager.Instance.PlaySFX(PauseSound);
    }


    public void SetInteractable(MonoBehaviour interactable)
    {
        currentInteractable = interactable;
    }


    // --- UPDATE ---
    // Aka. Animaciones y que esté tocando el suelo mientras no este muerto = no se pueda mover
    void Update()
    {
        if (!canMove) return;

        HandleGroundCheck();
        HandleMovementAnimations();
        HandleFootsteps();
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

    private void HandleFootsteps()
    {
        bool walking = Mathf.Abs(moveInput.x) > 0.1f && grounded;

        if (walking)
        {
            if (!footsteps.isPlaying)
                footsteps.Play();
        }
        else
        {
            if (footsteps.isPlaying)
                footsteps.Stop();
        }
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
}
