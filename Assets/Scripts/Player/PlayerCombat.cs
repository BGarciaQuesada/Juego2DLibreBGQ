using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Delay de ataque")]
    public float attackCooldown = 0.4f;

    [Header("Hitbox de ataque")]
    public GameObject attackHitbox;  // Desactivado por defecto, solo se activa al lanzar ataque

    private Animator anim;
    private bool canAttack = true; // Nuevamente, no spamear

    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitbox.SetActive(false); // por si acaso
    }

    // Input System
    void OnAttack(InputValue value)
    {
        if (!canAttack) return;

        Attack();
    }

    void Attack()
    {
        canAttack = false;

        anim.SetTrigger("Attack");

        // Activa hitbox después de un momento
        Invoke(nameof(EnableHitbox), 0.1f);
        Invoke(nameof(DisableHitbox), 0.25f);

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void EnableHitbox()
    {
        attackHitbox.SetActive(true);
    }

    void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}
