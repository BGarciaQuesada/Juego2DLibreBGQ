using TMPro;
using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    public static InteractionHint Instance;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TMP_Text hintText;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Comienza desactivado
        hintPanel.SetActive(false);
    }

    public void ShowHint(string message)
    {
        hintPanel.SetActive(true);
        hintText.text = message;
    }

    public void HideHint()
    {
        hintPanel.SetActive(false);
    }
}
