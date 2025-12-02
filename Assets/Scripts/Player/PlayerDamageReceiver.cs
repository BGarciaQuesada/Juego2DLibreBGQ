using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageReceiver : MonoBehaviour
{
    // La defensa actua como vida

    public float invulnTime = 0.5f;
    public float deathDelay = 4f;

    private bool canBeHit = true;
    private bool isDead = false;

    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // -> NOMBRE CAMBIADO PARA NO CONFUNDIR CON MÉTODO DE ENEMY
    public void PlayerTakeDamage(int rawDamage)
    {
        if (!canBeHit || isDead) return;

        canBeHit = false;

        // Restar defensa al recibir daño
        StatManager.Instance.ChangeStats(0, 0, -rawDamage, 0);

        anim.SetTrigger("Hurt");

        // ¿Ha muerto?
        if (StatManager.Instance.GetDefense() <= 0)
        {
            PlayerDie();
            return;
        }

        Invoke(nameof(ResetInvuln), invulnTime);
    }

    void ResetInvuln()
    {
        canBeHit = true;
    }

    void PlayerDie()
    {
        if (isDead) return;
        isDead = true;

        anim.SetTrigger("Die");

        // Quitar control
        var pm = GetComponent<PlayerMovement>();
        if (pm) pm.enabled = false;

        var pc = GetComponent<PlayerCombat>();
        if (pc) pc.enabled = false;


        // Detener movimiento físico
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        OutcomeUI.Instance.ShowMessage("<b>¡Derrota!</b>\nSerá mejor huir mientras puedas...");


        // Evitar daño
        canBeHit = false;

        // Volver a escena de entrenamiento
        StartCoroutine(ReturnToInbetween());
    }

    private System.Collections.IEnumerator ReturnToInbetween()
    {
        yield return new WaitForSeconds(deathDelay);

        SceneManager.LoadScene("Inbetween");
    }
}
