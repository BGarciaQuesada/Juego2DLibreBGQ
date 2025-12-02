using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    // Nuevamente, esto es básico, cada enemigo tiene vida distinta
    public int maxHP = 50;
    private int currentHP;

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isTakingDamage = false;

    protected Animator anim;
    protected EnemyBaseAI ai;

    [SerializeField] float delayBeforeReturn = 4f;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        ai = GetComponent<EnemyBaseAI>();
    }

    // -> NOMBRE CAMBIADO PARA NO CONFUNDIR CON MÉTODO DE PLAYER
    public virtual void EnemyTakeDamage(int dmg)
    {
        if (isDead || isTakingDamage) return;

        isTakingDamage = true;
        currentHP -= dmg;

        anim.SetTrigger("Hurt");

        if (currentHP <= 0)
        {
            EnemyDie();
            return;
        }

        Invoke(nameof(ResetHurt), 0.3f);
    }

    void ResetHurt()
    {
        isTakingDamage = false;
    }

    protected virtual void EnemyDie()
    {
        isDead = true;
        anim.SetTrigger("Death");

        // Desactivar colisión
        GetComponent<Collider2D>().enabled = false;

        // Dejar de mover el rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Ocultar
        StartCoroutine(FadeAndDisable());

        OutcomeUI.Instance.ShowMessage("<b>¡Victoria!</b>\nHora de regresar con la cabeza alta.");

        // De vuelta al entrenamiento
        StartCoroutine(ReturnToInbetween());
    }

    private IEnumerator ReturnToInbetween()
    {
        LevelManager.Instance.AdvanceLevel();
        yield return new WaitForSeconds(delayBeforeReturn);
        SceneManager.LoadScene("Inbetween");
    }

    private IEnumerator FadeAndDisable()
    {
        // Espera a que termine la animación de muerte
        yield return new WaitForSeconds(0.5f);

        // Oculpa el sprite (ya no se “mueve” porque no se ve)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        // Desactiva la IA (por si algo intenta ejecutarse)
        EnemyBaseAI ai = GetComponent<EnemyBaseAI>();
        if (ai != null) ai.enabled = false;
    }

}
