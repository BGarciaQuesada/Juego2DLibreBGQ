using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    public void RestartGame()
    {
        // Resetear stats
        StatManager.Instance.ResetStats();

        // Resetear progreso de niveles
        LevelManager.Instance.currentLevel = 0;

        // Cargar la primera escena del juego (ajústalo si es otra)
        SceneManager.LoadScene("Inbetween");
    }
}
