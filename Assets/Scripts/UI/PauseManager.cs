using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Esta clase maneja el menú de pausa y sus botones

    public static PauseManager Instance;

    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;

    // Desactivado al iniciar
    private void Awake()
    {
        Instance = this;
        pauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;   // Congela juego

        var pi = FindAnyObjectByType<PlayerInput>();
        pi.SwitchCurrentActionMap("UI");
    }

    // (Reanudar el tiempo en todas las opciones)
    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        var pi = FindAnyObjectByType<PlayerInput>();
        pi.SwitchCurrentActionMap("Player");
    }

    public void RestartBattle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitMainMenu()
    {
        Time.timeScale = 1f;
        // Tengo que crear el menu principal...
        SceneManager.LoadScene("Inbetween");
    }

    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

}
