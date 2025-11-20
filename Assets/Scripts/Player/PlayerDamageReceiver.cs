using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    // La defensa actua como vida

    public float invulnTime = 0.5f;
    private bool canBeHit = true;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int rawDamage)
    {
        if (!canBeHit) return;

        canBeHit = false;

        // Restar defensa al recibir daño
        StatManager.Instance.ChangeStats(0, 0, -rawDamage, 0);

        anim.SetTrigger("Hurt");

        // ¿Ha muerto?
        if (StatManager.Instance.GetDefense() <= 0)
        {
            Die();
            return;
        }

        Invoke(nameof(ResetInvuln), invulnTime);
    }

    void ResetInvuln()
    {
        canBeHit = true;
    }

    void Die()
    {
        anim.SetTrigger("Die");
        GetComponent<PlayerMovement>().enabled = false;
    }
}
