using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // Como cada escena tendrá música distinta, este método solo se 
    // dedidca a llamar al PlayMusic de AudioManager con dicha pista

    [SerializeField] AudioClip music;

    void Start()
    {
        if (music != null)
        {
            AudioManager.Instance.PlayMusic(music);
        }
    }
}
