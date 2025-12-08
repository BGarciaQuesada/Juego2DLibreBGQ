using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Escenas de combate")]
    public string[] levelScenes;

    [Header("Nivel a cargar")]
    public int currentLevel = 0;

    // Singleton
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

    public string GetNextLevel()
    {
        if (currentLevel < levelScenes.Length)
            return levelScenes[currentLevel];

        Debug.LogWarning("No quedan más niveles.");
        return null;
    }

    public void AdvanceLevel()
    {
        currentLevel++;
    }
}
