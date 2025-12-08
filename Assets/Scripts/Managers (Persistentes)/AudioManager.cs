using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Esta clase se encarga de manejar el volumen de todo el juego
    // (accesible desde menú principal)
    // BÁSICAMENTE -> Todo el audio hace "play" desde aquí, por lo que se conserva el volumen ajustado

    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float v)
    {
        musicSource.volume = v;
    }

    public void SetSFXVolume(float v)
    {
        sfxSource.volume = v;
    }

    // Esto solo sirve para efectos de sonido QUE SE REPRODUCEN 1 VEZ (Ataque, daño, etc)
    // Los loops (como andar) están en sus respectivas clases utilizando el valor de volumen de esta clase
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip newClip, float fadeTime = 1f)
    {
        if (musicSource.clip == newClip) return;

        StartCoroutine(FadeMusic(newClip, fadeTime));
    }

    // Hacer que desaparezca la música
    private System.Collections.IEnumerator FadeMusic(AudioClip newClip, float fadeTime)
    {
        float startVolume = musicSource.volume;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

}
