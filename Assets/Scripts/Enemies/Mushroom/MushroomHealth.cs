using UnityEngine;

public class MushroomHealth : MonoBehaviour
{
    public int maxHP = 50;
    private int currentHP;

    private Animator anim;
    private MushroomAI ai;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        ai = GetComponent<MushroomAI>();
    }

    public void TakeDamage(int dmg)
    {
        if (ai.isDead || ai.isTakingDamage)
            return;

        ai.isTakingDamage = true;
        currentHP -= dmg;

        anim.SetTrigger("Damaged");

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        Invoke(nameof(ResetHurt), 0.3f);
    }

    void ResetHurt()
    {
        ai.isTakingDamage = false;
    }

    void Die()
    {
        ai.isDead = true;
        anim.SetTrigger("Death");

        // Opcional: apagar colisiones
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2f);
    }
}
