using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Player       m_groundSensor;
    private bool                m_grounded = false;

    private Vector2             m_moveInput;
    private StatChange          nearbyInteractable;

    // Obtener componentes básicos
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("PlayerSensor").GetComponent<Sensor_Player>();
    }

    // --- Moverse ---
    void OnMove(InputValue value)
    {
        m_moveInput = value.Get<Vector2>();
    }

    // --- Saltar ---
    void OnJump()
    {
        if (m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
    }

    // --- Interactuar ---
    void OnInteract(InputValue value)
    {
        if (nearbyInteractable != null)
        {
            nearbyInteractable.Interact();
        }
    }

    public bool OnFloor()
    {
        return m_grounded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out StatChange interactable))
        {
            nearbyInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out StatChange interactable))
        {
            if (nearbyInteractable == interactable)
                nearbyInteractable = null;
        }
    }

    void Update()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = m_moveInput.x;
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);
        m_animator.SetFloat("AirSpeed", m_body2d.linearVelocity.y);
    }
}