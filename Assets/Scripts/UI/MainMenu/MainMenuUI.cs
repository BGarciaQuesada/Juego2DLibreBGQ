using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject mainPanel;
    public GameObject optionsPanel;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);

        // Cargar valores de audio guardados
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1f);
    }

    // --- BOTONES ---
    public void StartGame()
    {
        SceneManager.LoadScene("Inbetween");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // --- SLIDERS ---
    public void OnMusicChanged(float v)
    {
        AudioManager.Instance.SetMusicVolume(v);
        PlayerPrefs.SetFloat("MusicVol", v);
    }

    public void OnSFXChanged(float v)
    {
        AudioManager.Instance.SetSFXVolume(v);
        PlayerPrefs.SetFloat("SFXVol", v);
    }
}
