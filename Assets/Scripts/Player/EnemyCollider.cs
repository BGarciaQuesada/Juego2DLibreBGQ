using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollider : MonoBehaviour
{
    // La defensa va a actuar como vidas del jugador (cuando llega a 0, se queda KO)

    [Header("Sonidos")] [SerializeField] private AudioSource sonidoMuerte;
    [SerializeField] private AudioSource audioMuerte;

    [SerializeField] private float tiempoEspera; // Tiempo que todo se detiene

    private PlayerMovement playerMovement;

    private bool inmune = false;

    private int currentStrength, currentDefense;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // Obtener las estadísticas iniciales del jugador
        currentStrength = StatManager.Instance.GetStrength();
        currentDefense = StatManager.Instance.GetDefense();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            currentDefense--;
            // Esto está manejado en PlayerMovement, hay que cambiarlo
            playerAnimations.AnimacionMuerte();
            sonidoMuerte.Play();
            Debug.Log("Defensa restante: " + currentDefense);
            if (currentDefense <= 0)
            {
                StartCoroutine(PararYReiniciar());
            }
        }
    }

    private IEnumerator PararYReiniciar()
    {
        Time.timeScale = 0f; // Parar todo (EXCEPTO GIRO DEL PLAYER)
        if(playerMovement != null)
            playerMovement.enabled = false; // Desactivar el movimiento del jugador

        playerAnimations.AnimacionVida();

        yield return new WaitForSecondsRealtime(tiempoEspera); // Esperar un tiempo
        // Recargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f; // Reanudar el tiempo
    }
}
