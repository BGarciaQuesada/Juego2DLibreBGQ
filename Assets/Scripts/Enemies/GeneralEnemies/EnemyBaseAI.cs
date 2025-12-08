using System.Collections;
using UnityEngine;

public abstract class EnemyBaseAI : MonoBehaviour
{
    // Aka. Creo una clase padre AI donde se maneja el daño
    // y cada hijo lo hereda para poner su propio patrón de movimiento

    [Header("Sonidos Combate")]
    [SerializeField] AudioClip EnemyAttack;

    // Esto es por poner algo básico pero cada hijo tiene un daño distinto
    [Header("Daño")]
    public int attackDamage = 5;

    [SerializeField] protected float attackDelay = 0.4f; // tiempo hasta el golpe real

    protected bool isDead = false;
    protected bool isTakingDamage = false;
    protected bool isAttacking = false;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected Collider2D playerInRange;

    protected virtual void Start()
    {
        // Obtener componentes básicos...
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (isDead || isTakingDamage)
        {
            // Si está muerto quieto
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // Sino, que siga
        AIBehaviour();
    }

    protected abstract void AIBehaviour();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || isTakingDamage) return;

        if (other.CompareTag("Player"))
        {
            playerInRange = other;
            if (!isAttacking)
                StartCoroutine(PerformAttack());
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        if (playerInRange == other)
            playerInRange = null;
    }
}


    public virtual void ReceiveDamage(int dmg)
    {
        // ¿Existe? Recibe daño del jugador
        GetComponent<EnemyHealth>()?.EnemyTakeDamage(dmg);
    }

    protected IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Ejecutar animación
        anim.SetTrigger("Attack");

        AudioManager.Instance.PlaySFX(EnemyAttack);

        // Esperar para la animación
        yield return new WaitForSeconds(attackDelay);

        // Si el jugador ya no está en el rango, no se aplica daño
        if (playerInRange != null)
        {
            PlayerDamageReceiver player =
                playerInRange.GetComponent<PlayerDamageReceiver>();

            if (player != null)
                player.PlayerTakeDamage(attackDamage);
        }

        isAttacking = false;
    }

}
