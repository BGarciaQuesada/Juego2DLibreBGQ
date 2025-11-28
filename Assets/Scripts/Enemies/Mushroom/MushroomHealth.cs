using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MushroomHealth : MonoBehaviour
{
    public int maxHP = 50;
    private int currentHP;

    private Animator anim;
    private MushroomAI ai;

    [SerializeField] float delayBeforeReturn = 4f; // tiempo antes de volver

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

        anim.SetTrigger("Hurt");

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
        Debug.Log("EJECUTADO DIE EN MUSHROMHEALTH");
        ai.isDead = true;
        anim.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(ReturnToInbetween());
    }

    private IEnumerator ReturnToInbetween()
    {
        Debug.Log("La seta a muerto, esperando");

        // Avanzamos el nivel
        LevelManager.Instance.AdvanceLevel();

        // Esperamos el tiempo programado
        yield return new WaitForSeconds(delayBeforeReturn);

        Debug.Log("De vuelta...");

        // Volvemos a la escena Inbetween
        SceneManager.LoadScene("Inbetween");
    }
}
